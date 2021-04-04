using FlightManager.Data;
using FlightManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Services.Contracts
{
    public interface IFlightsService
    {
        public Flight CreateFlight(Flight model);
        public Flight DeleteFlight(Flight flight);
        public Flight GetFlight(string destinationCity, DateTime departureTime, string captainName);
        public Flight GetFlightById(int id);
        public List<Flight> GetAllFlights();
        public Flight UpdateFlight(Flight model);

    }
}
