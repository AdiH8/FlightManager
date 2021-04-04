using FlightManager.Data.Models;
using System;
using System.Collections.Generic;

namespace FlightManager.Data.Models
{
    public class ReservationsIndexViewModel
    {
        public List<ReservationIndexViewModel> Reservations { get; set; }
        public string Filter { get; set; }
        public int PagesCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
