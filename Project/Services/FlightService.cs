using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Services
{
    public class FlightService : IFlightsService
    {
        private readonly ApplicationDbContext dBContext;

        public FlightService(ApplicationDbContext context)
        {
            dBContext = context;
        }

        public Flight CreateFlight(Flight model)
        {
            Flight flight = new Flight() {
                GoingTo = model.GoingTo,
                LeavingFrom = model.LeavingFrom,
                Departure = model.Departure,
                Arrival = model.Arrival,
                PilotName = model.PilotName,
                AirplaneType = model.AirplaneType,
                PassengersCapacity = model.PassengersCapacity,
                BusinessClassCapacity = model.BusinessClassCapacity,
                BusinessTicketsLeft = model.BusinessClassCapacity,
                TicketsLeft = model.PassengersCapacity,
                PlaneId = model.PlaneId
            };
           
          
            dBContext.Flights.Add(flight);
            dBContext.SaveChanges();

            return flight;
        }

        public Flight DeleteFlight(Flight flight)
        {
            dBContext.Flights.Remove(flight);
            dBContext.SaveChanges();

            return flight;
        }

        public List<Flight> GetAllFlights()
        {
            return dBContext.Flights.ToList();
        }

        public Flight GetFlight(string destinationCity, DateTime departureTime, string departureCity)
        {
            return dBContext.Flights.Where(f => f.GoingTo == destinationCity && f.Departure == departureTime && f.LeavingFrom == departureCity).First();
        }

        public Flight GetFlightById(int id) { 

            return dBContext.Flights.Where(f => f.Id == id).First();
        }

        public Flight UpdateFlight(Flight model)
        {
            Flight dbFlight = dBContext.Flights.Where(f => f.Id == model.Id).First();

            dbFlight.Arrival = model.Arrival;
            dbFlight.BusinessClassCapacity = model.BusinessClassCapacity;
            dbFlight.PilotName = model.PilotName;
            dbFlight.LeavingFrom = model.LeavingFrom;
            dbFlight.Departure = model.Departure;
            dbFlight.GoingTo = model.GoingTo;
            dbFlight.PassengersCapacity = model.PassengersCapacity;
            dbFlight.PlaneId = model.PlaneId;
            dbFlight.AirplaneType = model.AirplaneType;

            dBContext.Flights.Update(dbFlight);
            dBContext.SaveChanges();

            return dbFlight;
        }
    }
}
