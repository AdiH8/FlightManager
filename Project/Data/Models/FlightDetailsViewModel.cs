using System;
using System.Collections.Generic;

namespace FlightManager.Data.Models
{
    public class FlightDetailsViewModel
    {
        public int FlightId { get; set; }
        public string DestinationCity { get; set; }
        public string DepartureCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan FlightDuration { get; set; }
        public string PlaneType { get; set; }
        public string PilotName { get; set; }
        public int PlaneCapacity { get; set; }
        public int TicketsLeft { get; set; }
        public int BusinessClassCapacity { get; set; }
        public int BusinessTicketsLeft { get; set; }
        public List<FlightReservationViewModel> Reservations { get; set; }
        public int PagesCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
