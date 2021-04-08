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
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Social Security Number")]
        public string SSN { get; set; }
        public Flight Flight  { get; set; }

        [Display(Name = "Flight ID")]
        public int FlightID  { get; set; }

        [Required]
        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; }

        [Display(Name = "Confirmed Reservation")]
        public bool IsConfirmed { get; set; }

        public int TicketsCount { get; set; }
        
    }
}
