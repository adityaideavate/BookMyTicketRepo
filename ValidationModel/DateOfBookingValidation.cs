using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket.ValidationModel
{
    public class DateOfBookingValidation: ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var booking = (Booking)validationContext.ObjectInstance;


            if (booking.DateOfBooking <= DateTime.Now)
            {

                return new ValidationResult("Please provide valid date");

            }
            else
            {
                return ValidationResult.Success;
            }


       }

    }
}