using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyTicket.ViewModel
{
    public class BookingViewModel
    {
        public Booking booking { get; set; }
        public Show show { get; set; }
        public List<Payment> paymentlist { get; set; }
        public Screen screen { get; set; }
    }
}