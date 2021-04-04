using Microsoft.AspNetCore.Identity;
using FlightManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data
{
    public class ApplicationUser : IdentityUser
    {
        
        public string FirstName { get; set; }
        public string Surname { get; set; }

        [StringLength(10)]
        public string SSN { get; set; }
        public string Address { get; set; }

    }
}
