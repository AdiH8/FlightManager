using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Data.Models
{
    public class FlightBooking : BaseEntity
    {

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public string Nationality { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Social Security Number")]
        public string SSN { get; set; }
        public Flight Flight  { get; set; }

        [Display(Name = "Flight ID")]
        
        public int FlightID  { get; set; }
        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; }
        [Display(Name = "Confirmed Reservation")]
        public bool IsConfirmed { get; set; }

        public int TicketsCount { get; set; }
        
    }
}
