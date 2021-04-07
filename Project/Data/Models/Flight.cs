using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Models
{
    public class Flight : BaseEntity
    {
        [Required]
        [Display(Name ="Leaving From")]
        public string LeavingFrom { get; set; }
        [Required]
        [Display(Name = "Going To")]
        public string GoingTo { get; set; }

        [Display(Name = "Plane Type")]
        public string AirplaneType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Plane Capacity cannot be negative or 0.")]
        [Display(Name = "Plane Capicity")]
        public int PassengersCapacity { get; set; }
        [Display(Name = "Tickets Left")]
        public int TicketsLeft { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Business Class Capacity cannot be negative or 0.")]
        [Display(Name = "Business Class Capacity")]
        public int BusinessClassCapacity { get; set; }

        [Display(Name = "Business Tickets Left")]
        public int BusinessTicketsLeft { get; set; }

        [Display(Name = "Plane Id")]
        [Range(1, int.MaxValue, ErrorMessage = "The Plane ID cannot be negative.")]
        public int PlaneId { get; set; }

        [Display(Name = "Departure Time")]
        public DateTime Departure { get; set; }

        [Display(Name = "Time of Arrival")]
        public DateTime Arrival { get; set; }
        [Display(Name = "Pilot Name")]
        public string PilotName { get; set; }
    }
}
