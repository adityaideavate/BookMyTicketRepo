using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyTicket.ViewModel
{
    public class ScreenViewModel
    {
        public Screen screen { get; set; }
        public List<Theatre> theatreList { get; set; }
        public List<City> cityList { get; set; }
    }
}