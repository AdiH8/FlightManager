using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Models
{
    public class ReservationCreateViewModel
    {
        public List<ReservationFlightCreateViewModel> Flights { get; set; }
        public int FlightId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Tickets To Buy")]
        public int TicketCount { get; set; }
        public int TicketsLeft { get; set; }
        public int BusinessTicketsLeft { get; set; }

        [Required]
        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; }
        public string ErrorMessage { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Social Security Number")]
        public string SSN { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Nationality { get; set; }
    }
}
