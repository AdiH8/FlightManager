using System;

namespace FlightManager.Data.Models
{
    public class ReservationIndexViewModel
    {
        public int ReservationId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string TicketType { get; set; }
        public string Email { get; set; }
        public string DestinationCity { get; set; }
        public string DepartureCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public int TicketsCount { get; set; }
    }
}
