using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Data.Models
{
    public class ReservationConfirmViewModel
    {
        public string DepartureCity { get; set; }
        public string DestinationCity { get; set; }
        public string Name { get; set; }
    }
}
