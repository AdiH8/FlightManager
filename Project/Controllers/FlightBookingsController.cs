using System;
using System.Linq;
using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.Services;
using FlightManager.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FlightManager.Controllers
{
    public class FlightBookingsController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IFlightsService flightService;

        public FlightBookingsController(IReservationService reservationService, IFlightsService flightService)
        {
            this.reservationService = reservationService;
            this.flightService = flightService;
        }

        // gets all the reservations from the database
        public IActionResult Index(string filter, int? page, int pageSize = 10)
        {
            int pageNumber = (page ?? 1);
            ReservationsIndexViewModel model = new ReservationsIndexViewModel()
            {
                Reservations = reservationService.GetAllReservations().Select(r => new ReservationIndexViewModel()
                {
                    ReservationId=r.Id,
                    Name = r.FirstName + " " + r.Surname,
                    Email = r.Email,
                    DepartureCity = flightService.GetFlightById(r.FlightID).LeavingFrom,
                    DepartureTime = flightService.GetFlightById(r.FlightID).Departure,
                    DestinationCity = flightService.GetFlightById(r.FlightID).GoingTo,
                    PhoneNumber = r.PhoneNumber,
                    TicketType = r.TicketType,
                    TicketsCount = r.TicketsCount,
                    ConfirmedReservation=r.IsConfirmed
                }).ToList(),
                Filter = filter,
                PageNumber = pageNumber,
                PagesCount = (int)Math.Ceiling(reservationService.GetAllReservations().Count / (double)pageSize),
                PageSize = pageSize
            };

            // orders and re-orders the reservations if chosen to be sorted ascending or descending by email
            if (filter == "email")
            {
                model.Reservations = model.Reservations.OrderBy(r => r.Email).ToList();
            }
            else if (filter == "emailReversed")
            {
                model.Reservations = model.Reservations.OrderByDescending(r => r.Email).ToList();
            }

            // here is realized changing the pages
            model.Reservations = model.Reservations.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(model);
        }

        // this method is called from the Index view in the Flight folder
        // when  an exact flight is wanted to be booked
        public IActionResult Book(int id)
        {
            ViewBag.Message = id;
            return View("Book");
        }

        [HttpPost]
        public IActionResult Book(ReservationCreateViewModel model)
        {
            Flight flight = flightService.GetFlightById(model.FlightId);

            // checks if there are enough tickets left
            if (model.TicketType == "Business" && flight.BusinessTicketsLeft == 0)
            {
                return View("FullPlaneView");
            }
            else if (model.TicketType == "Regular" && flight.TicketsLeft == 0)
            {
                return View("FullPlaneView");
            }

            ReservationCreateViewModel resModel = new ReservationCreateViewModel()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                LastName = model.LastName,
                SSN = model.SSN,
                PhoneNumber = model.PhoneNumber,
                Nationality = model.Nationality,
                TicketType = model.TicketType,
                TicketCount = 1,
                BusinessTicketsLeft = flight.BusinessTicketsLeft,
                FlightId = flight.Id,
                TicketsLeft = flight.TicketsLeft
            };

            reservationService.CreateReservation(resModel);

            return View("SuccessfulReservation");
        }

        public IActionResult Create()
        {
            ReservationCreateViewModel model = new ReservationCreateViewModel()
            {
                Flights = flightService.GetAllFlights().Select(f => new ReservationFlightCreateViewModel()
                {
                    FlightID = f.Id,
                    DepartureCity = f.LeavingFrom,
                    DepartureTime = f.Departure,
                    DestinationCity = f.GoingTo,
                    BusinessTicketsLeft = f.BusinessTicketsLeft,
                    TicketsLeft = f.TicketsLeft
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ReservationCreateViewModel model)
        {
            Flight flight = flightService.GetFlightById(model.FlightId);

            // checks if there are enough tickets left
            if (model.TicketType == "Business" && flight.BusinessTicketsLeft ==0)
            {
                return View("FullPlaneView");
            }
            else if (model.TicketType == "Regular" && flight.TicketsLeft ==0)
            {
                return View("FullPlaneView");
            }

            ReservationCreateViewModel resModel = new ReservationCreateViewModel()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                LastName = model.LastName,
                SSN = model.SSN,
                PhoneNumber = model.PhoneNumber,
                Nationality = model.Nationality,
                TicketType = model.TicketType,
                TicketCount = 1,
                BusinessTicketsLeft = flight.BusinessTicketsLeft,
                FlightId = flight.Id,
                TicketsLeft = flight.TicketsLeft
            };

            reservationService.CreateReservation(resModel);

            return View("SuccessfulReservation");
        }

        public IActionResult Confirm(int id)
        {
            if (reservationService.GetReservationById(id) == null)
            {
                return View("NotFound");
            }

            reservationService.ConfirmReservation(id);

            FlightBooking reservation = reservationService.GetReservationById(id);

            ReservationConfirmViewModel model = new ReservationConfirmViewModel()
            {
                DepartureCity = reservation.Flight.LeavingFrom,
                DestinationCity = reservation.Flight.GoingTo,
                Name = reservation.FirstName + " " + reservation.Surname
            };

            return View(model);
        }

        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var reservation = reservationService.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
      
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var reservation = reservationService.GetReservationById(id);
            if (reservation == null)
            { 
                return View("NotFound");
            }

            // checks if the reservation is confirmed
            if (reservation.IsConfirmed)
            {
                return View("CannotDelete");
            }

           

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            reservationService.DeleteReservation(id);
            return RedirectToAction(nameof(Index));
        }
    }
}