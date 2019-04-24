using BookMyTicket.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket.ValidationModel
{
    public class NoOfSeatLeftValidation : ValidationAttribute
    {

        AdityaEntities4 db = new AdityaEntities4();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {


            var bvm = (Booking)validationContext.ObjectInstance;




            using (var context = new AdityaEntities4())

            {


                var objData = from f in context.Bookings
                              where f.DateOfBooking == bvm.DateOfBooking
                              && f.ShowId == bvm.ShowId
                              group f by new { f.DateOfBooking, f.ShowId } into t
                              select new
                              {
                                  /*   dateofBooking = t.Key.DateOfBooking,
                                     showId = t.Key.ShowId,           */
                                  TotalNoOfSeats = t.Sum(s => s.NoOfSeats)
                              };


                int numberOfSeats = 0;

                foreach (var a in objData)
                {
                    numberOfSeats = a.TotalNoOfSeats;
                }


                var show = db.Shows.SingleOrDefault(temp=>temp.ShowId == bvm.ShowId);

                  var var1= show.ScreenId;


                var screenindb = db.Screens.SingleOrDefault(temp=>temp.ScreenId== var1);


                int leftseats = screenindb.ScreenCapacity - numberOfSeats;


             //   int leftseats = bvm.screen.ScreenCapacity - numberOfSeats;


                if (bvm.NoOfSeats > leftseats)
                {

                    return new ValidationResult(leftseats +" seats are left");

                    // ViewBag.leftseatsStatus1 = leftseats;

                }
                else
                {
                    return ValidationResult.Success;

                }


            }




        }


    }
}