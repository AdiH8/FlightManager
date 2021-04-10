using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.Services.Contracts;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext dBContext;
        private readonly IEmailSender emailSender;

        public ReservationService(ApplicationDbContext context, IEmailSender emailSender)
        {
            dBContext = context;
            this.emailSender = emailSender;
        }

        public FlightBooking ChangeTicketType(FlightBooking reservation)
        {
            dBContext.FlightBookings.Where(r => r.Id == reservation.Id).First().TicketType = reservation.TicketType == "Business" ? "Regular" : "Business";
            dBContext.SaveChanges();

            return reservation;
        }

        public List<FlightBooking> GetAllReservations()
        {
            return dBContext.FlightBookings.ToList();
        }

        public List<FlightBooking> GetAllReservationsForFlight(Flight flight)
        {
            return dBContext.FlightBookings.Where(r => r.Flight.Id == flight.Id).ToList();
        }

        public FlightBooking CreateReservation(ReservationCreateViewModel model)
        {
            
            FlightBooking reservation = new FlightBooking()
            {
                TicketType = model.TicketType,
                Flight = dBContext.Flights.Where(f => f.Id == model.FlightId).First(),
                IsConfirmed = false,
                TicketsCount =1,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.SecondName,
                Surname = model.LastName,
                SSN = model.SSN,
                PhoneNumber = model.PhoneNumber,
                Nationality = model.Nationality,
                FlightID = model.FlightId

            };

            // changes the count of the left tickets for the flight
            Flight dbFlight = dBContext.Flights.Where(f => f.Id.ToString() == model.FlightId.ToString()).First();
            if (model.TicketType == "Business")
            {
                dbFlight.BusinessTicketsLeft -= model.TicketCount;
            }
            else
            {
                dbFlight.TicketsLeft -= model.TicketCount;
            }
            
            dBContext.FlightBookings.Add(reservation);
            dBContext.SaveChanges();

            // here is the message that is automatically send to the given email address when done with making the reservation
            // note that the links below contain the address of a local host, currently on the computer that the program is running on
            // in order this to works on another device, the address have to be changed with the numbers of the different local host
            string msg = $@"Dear Mr./Miss {reservation.Surname}, <br>
                            Thank you for using our services! <br> You have successfuly made a reservation 
                            for flight No.{dbFlight.Id} from {dbFlight.LeavingFrom} to {dbFlight.GoingTo} leaving on {dbFlight.Departure}.
                            Now it's only left for you to confirm it. <br />
                            <a href={"https://localhost:44378"}/FlightBookings/Confirm?id={reservation.Id}>Confirm Reservation</a> <br />
                            <a href={"https://localhost:44378"}/FlightBookings/Delete?id={reservation.Id}>Delete</a>";

            // sends the message to the given email address
            emailSender.SendEmailAsync(reservation.Email, "Reservation Confirmation", msg).GetAwaiter().GetResult();

            return reservation;
        }

        public FlightBooking ConfirmReservation(int id) { 
            FlightBooking reservation = dBContext.FlightBookings.Where(r => r.Id == id).First();
            reservation.IsConfirmed = true;

            dBContext.FlightBookings.Update(reservation);
            dBContext.SaveChanges();

            return reservation;
        }

        public FlightBooking DeleteReservation(int id)
        {
            FlightBooking reservation = dBContext.FlightBookings.Where(r => r.Id == id).First();

            // you can only delete not confirmed reservations
            // however when deleting a flight that has confirmed reservations for it,
            // an email is send to every passenger about what has occured and why their reservation has been deleted
            if (reservation.IsConfirmed) {
                string msg = $@"We are sorry to inform you that there have been some issues 
                 with flight No. {reservation.Flight.Id} - {reservation.Flight.LeavingFrom} 
                 to {reservation.Flight.GoingTo}, which led to its cancelation and the termination of your reservation. <br>
                 Please contact us to choose another flight to book 
                 or to have your money returned. <br> <br>
                 Thank you for using our services! <br> <br> Cloud Express";

                emailSender.SendEmailAsync(reservation.Email, "Canceled Reservation", msg).GetAwaiter().GetResult();
            }

            Flight dbFlight = dBContext.Flights.Where(f => f.Id.ToString() == reservation.FlightID.ToString()).First();
           
            // increases the count of the tickets left for the particular flight and class
            if (reservation.TicketType == "Business")
            {
                dbFlight.BusinessTicketsLeft += 1;
            }
            else
            {
                dbFlight.TicketsLeft +=1;
            }


            dBContext.FlightBookings.Remove(reservation);
            dBContext.SaveChanges();

            return reservation;
        }

        public FlightBooking GetReservationById(int id)
        {
            FlightBooking reservation = new FlightBooking();
            try
            {
                reservation = dBContext.FlightBookings.Where(r => r.Id == id).First();
            }
            catch (Exception)
            {

                return null;
            }
           
            return reservation;
        }
    }
}
