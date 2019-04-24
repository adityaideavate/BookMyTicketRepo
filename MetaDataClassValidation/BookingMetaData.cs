    using BookMyTicket.ValidationModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket
{
  

    [MetadataType(typeof(BookingMetaData))]
    public partial class Booking
    {
       // public string Email { get; set; }
    }


    public class BookingMetaData
    {
        //[BookingValidation]
        //[Required]
        //[StringLength(50, ErrorMessage = "Max 50 characters")]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter correct email address")]
        //public string Email { get; set; }

   //     [NoOfSeatLeftValidation]
         [Range(1, 10, ErrorMessage = " Select seats between 1 to 10")]
         public int NoOfSeats { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime DateOfBooking { get; set; }

        //     [DateOfBookingValidation]
        //     public System.DateTime DateOfBooking { get; set; }

    }
}