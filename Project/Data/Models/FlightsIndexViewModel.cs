using System.Collections.Generic;

namespace FlightManager.Data.Models
{
    public class FlightsIndexViewModel
    {
        public List<FlightIndexViewModel> Flights { get; set; }
        public int PagesCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
