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
        private readonly ApplicationDbContext _context;

        public FlightBookingsController(IReservationService reservationService, IFlightsService flightService)
        {
            this.reservationService = reservationService;
            this.flightService = flightService;
        }

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
                    DepartureCity = flightService.GetFlightById(r.FlightId).LeavingFrom,
                    DepartureTime = flightService.GetFlightById(r.FlightId).Departure,
                    DestinationCity = flightService.GetFlightById(r.FlightId).GoingTo,
                    PhoneNumber = r.PhoneNumber,
                    TicketType = r.TicketType,
                    TicketsCount = r.TicketsCount
                }).ToList(),
                Filter = filter,
                PageNumber = pageNumber,
                PagesCount = (int)Math.Ceiling(reservationService.GetAllReservations().Count / (double)pageSize),
                PageSize = pageSize
            };

            if (filter == "email")
            {
                model.Reservations = model.Reservations.OrderBy(r => r.Email).ToList();
            }
            else if (filter == "emailReversed")
            {
                model.Reservations = model.Reservations.OrderByDescending(r => r.Email).ToList();
            }

            model.Reservations = model.Reservations.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(model);
        }
        public IActionResult Book(int id)
        {
            ViewBag.Message = id;
            return View("Book");
        }

        [HttpPost]
        public IActionResult Book(ReservationCreateViewModel model)
        {
            Flight flight = flightService.GetFlightById(model.FlightId);
            if (model.TicketType == "Business" && flight.BusinessTicketsLeft == 0)
            {
                model.ErrorMessage = "Not enough Business class tickets!";
                return View("Create", model);
            }
            else if (model.TicketType == "Regular" && flight.TicketsLeft ==0 )
            {
                model.ErrorMessage = "Not enough Regular class tickets!";
                return View("Create", model);
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

            return RedirectToAction("Index", "Home");
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
            if (model.TicketType == "Business" && flight.BusinessTicketsLeft ==0)
            {
               // model.ErrorMessage = "Not enough Business class tickets!";
                
                return View("FullPlaneView");
            }
            else if (model.TicketType == "Regular" && flight.TicketsLeft ==0)
            {
               
               // model.ErrorMessage = "Not enough Regular class tickets!";
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
                TicketCount = model.TicketCount,
                BusinessTicketsLeft = flight.BusinessTicketsLeft,
                FlightId = flight.Id,
                TicketsLeft = flight.TicketsLeft
            };

            reservationService.CreateReservation(resModel);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Confirm(int id)
        {
            if (reservationService.GetReservationById(id) == null)
            {
                return RedirectToAction("NotFound");
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

            var reservation = _context.FlightBookings
                .FirstOrDefault(m => m.Id == id);

            if (reservation.IsConfirmed)
            {
                return View("CannotDelete");
            }
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var reservation =  _context.FlightBookings.Find(id);
            _context.FlightBookings.Remove(reservation);
             _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}