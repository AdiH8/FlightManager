using System;

namespace FlightManager.Data.Models
{
    public class ReservationFlightCreateViewModel
    {
        public int FlightID { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureCity { get; set; }
        public string DestinationCity { get; set; }
        public int TicketsLeft { get; set; }
        public int BusinessTicketsLeft { get; set; }
    }
}
