using BookMyTicket.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket.ValidationModel
{ 
     
    public class BookingValidation : ValidationAttribute
    {
        AdityaEntities4 db = new AdityaEntities4();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {           

            var booking = (Booking)validationContext.ObjectInstance;

                       

            var emailindb = db.AspNetUsers.SingleOrDefault(temp => temp.Email == booking.Email);

            if (emailindb == null)
            {
                return new ValidationResult("Please provide registered email address");
            }
            else
            {
                return ValidationResult.Success;

            }
                               
            }
    }
}