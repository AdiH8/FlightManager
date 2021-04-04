using FlightManager.Data;
using FlightManager.Data.Models;
using System;
using System.Collections.Generic;

namespace FlightManager.Services.Contracts
{
    public interface IReservationService
    {
        public FlightBooking CreateReservation(ReservationCreateViewModel model);
        public FlightBooking DeleteReservation(int id);
        public FlightBooking GetReservationById(int id);
        public List<FlightBooking> GetAllReservations();
        public List<FlightBooking> GetAllReservationsForFlight(Flight flight);
        public FlightBooking ConfirmReservation(int id);
        public FlightBooking ChangeTicketType(FlightBooking reservation);
    }
}
