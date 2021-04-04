using System;

namespace FlightManager.Data.Models
{
    public class FlightIndexViewModel
    {
        public int FlightId { get; set; }
        public string DestinationCity { get; set; }
        public string DepartureCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan TravelTime { get; set; }
        public string PlaneType { get; set; }
        public int PlaneID { get; set; }
        public string PilotName { get; set; }
        public int PlaneCapacity { get; set; }
        public int BusinessClassCapacity { get; set; }
    }
}
