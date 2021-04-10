using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.Services;
using FlightManager.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightsService flightService;
        private readonly IReservationService reservationService;

        public FlightsController(IFlightsService flightService, IReservationService reservationService)
        {
            this.flightService = flightService;
            this.reservationService = reservationService;
        }

       
        public IActionResult Index(string searchString, int? page, int pageSize = 10)
        {
            int pageNumber = (page ?? 1);
            FlightsIndexViewModel model = new FlightsIndexViewModel()
            {
                Flights = flightService.GetAllFlights().Select(f => new FlightIndexViewModel()
                {
                    FlightId = f.Id,
                    TravelTime = f.Arrival.Subtract(f.Departure),
                    BusinessClassCapacity = f.BusinessClassCapacity,
                    PilotName = f.PilotName,
                    DepartureCity = f.LeavingFrom,
                    DepartureTime = f.Departure,
                    DestinationCity = f.GoingTo,
                    PlaneID=f.PlaneId,
                    PlaneCapacity = f.PassengersCapacity,
                    PlaneType = f.AirplaneType
                }).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = (int)Math.Ceiling(flightService.GetAllFlights().Count / (double)pageSize)
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                model.Flights = model.Flights.Where(f => f.DepartureCity.Contains(searchString) || f.DestinationCity.Contains(searchString)).ToList();
            }

            model.Flights = model.Flights.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(model);
        }

        public IActionResult Delete(int id, int? page, int pageSize = 10)
        {
            int pageNumber = (page ?? 1);
            Flight flight = flightService.GetFlightById(id);
            FlightDetailsViewModel model = new FlightDetailsViewModel()
            {
                FlightId = flight.Id,
                BusinessClassCapacity = flight.BusinessClassCapacity,
                PilotName = flight.PilotName,
                DepartureCity = flight.LeavingFrom,
                DepartureTime = flight.Departure,
                DestinationCity = flight.GoingTo,
                FlightDuration = flight.Arrival.Subtract(flight.Departure),
                PlaneCapacity = flight.PassengersCapacity,
                PlaneType = flight.AirplaneType,
                BusinessTicketsLeft = flight.BusinessTicketsLeft,
                TicketsLeft = flight.TicketsLeft,
                Reservations = reservationService.GetAllReservationsForFlight(flight).Select(r => new FlightReservationViewModel()
                {
                    Email = r.Email,
                    Name = r.FirstName + " " + r.MiddleName + " " + r.Surname,
                    Nationality = r.Nationality,
                    PhoneNumber = r.PhoneNumber,
                    SSN = r.SSN,
                    TicketType = r.TicketType,
                    TicketsCount = r.TicketsCount
                }).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = (int)Math.Ceiling(reservationService.GetAllReservationsForFlight(flight).Count / (double)pageSize)
            };

            model.Reservations = model.Reservations.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            return View(model);
        }


        [HttpPost]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Flight flightToDelete = flightService.GetFlightById(id);
            List<FlightBooking> Reservations = reservationService.GetAllReservationsForFlight(flightToDelete);

            // in order to delete a flight, we have to also delete all the reservations coming with it
            if (Reservations.Count!=0)
            {
                foreach (var reservation in Reservations)
                {
                    reservationService.DeleteReservation(reservation.Id);
                }
            }
            flightService.DeleteFlight(flightToDelete);

            return RedirectToAction("Index");
        }

        
      
       
        public IActionResult Details(int id, int? page, int pageSize = 10)
        {
            int pageNumber = (page ?? 1);
            Flight flight = flightService.GetFlightById(id);
            FlightDetailsViewModel model = new FlightDetailsViewModel()
            {
                FlightId = flight.Id,
                BusinessClassCapacity = flight.BusinessClassCapacity,
                PilotName = flight.PilotName,
                DepartureCity = flight.LeavingFrom,
                DepartureTime = flight.Departure,
                DestinationCity = flight.GoingTo,
                FlightDuration = flight.Arrival.Subtract(flight.Departure),
                PlaneCapacity = flight.PassengersCapacity,
                PlaneType = flight.AirplaneType,
                BusinessTicketsLeft = flight.BusinessTicketsLeft,
                TicketsLeft = flight.TicketsLeft,
                Reservations = reservationService.GetAllReservationsForFlight(flight).Select(r => new FlightReservationViewModel()
                {
                    Email = r.Email,
                    Name = r.FirstName + " " + r.MiddleName + " " + r.Surname,
                    Nationality = r.Nationality,
                    PhoneNumber = r.PhoneNumber,
                    SSN = r.SSN,
                    TicketType = r.TicketType,
                    TicketsCount = r.TicketsCount
                }).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = (int)Math.Ceiling(reservationService.GetAllReservationsForFlight(flight).Count / (double)pageSize)
            };

            model.Reservations = model.Reservations.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public IActionResult Create(Flight model)
        {
            if (model.Arrival < model.Departure)
            {
                ModelState.AddModelError("Departure Time", "Departure time should be a date/time before Arrival time.");
                return View(model);
            }
            flightService.CreateFlight(model);
            return RedirectToAction("Index");
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            Flight flight = flightService.GetFlightById(id);

            Flight model = new Flight()
            {
                Id=flight.Id,
                Arrival = flight.Arrival,
                BusinessClassCapacity = flight.BusinessClassCapacity,
                PassengersCapacity = flight.PassengersCapacity,
                PlaneId = flight.PlaneId,
                AirplaneType = flight.AirplaneType,
                PilotName = flight.PilotName,
                LeavingFrom = flight.LeavingFrom,
                Departure = flight.Departure,
                GoingTo = flight.GoingTo
               
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Flight model)
        {
            Flight flight = new Flight()
            {
                Id = model.Id,
                Arrival = model.Arrival,
                BusinessClassCapacity = model.BusinessClassCapacity,
                PilotName = model.PilotName,
                LeavingFrom = model.LeavingFrom,
                Departure = model.Departure,
                GoingTo = model.GoingTo,
                PassengersCapacity = model.PassengersCapacity,
                PlaneId = model.PlaneId,
                AirplaneType = model.AirplaneType
            };

            flightService.UpdateFlight(model);

            return RedirectToAction("Details");
        }

    }
}
