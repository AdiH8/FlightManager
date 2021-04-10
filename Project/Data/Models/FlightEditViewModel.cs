using System;

namespace FlightManager.Data.Models
{
    public class FlightEditViewModel
    {
        public int FlightId { get; set; }
        public string DestinationCity { get; set; }
        public string DepartureCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string PlaneType { get; set; }
        public int PlaneID { get; set; }
        public string CaptainName { get; set; }
        public int PlaneCapacity { get; set; }
        public int TicketsLeft { get; set; }
        public int BusinessClassCapacity { get; set; }
        public int BusinessTicketsLeft { get; set; }
    }
}
