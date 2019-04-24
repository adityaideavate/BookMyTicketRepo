using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyTicket.ViewModel
{
    public class TheatreViewModel
    {
        public Theatre theatre { get; set; }
        public List<City> cityList { get; set; }
    }
}