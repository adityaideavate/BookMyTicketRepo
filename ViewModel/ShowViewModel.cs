
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyTicket.ViewModel
{
    public class ShowViewModel
    {

        public Show show { get; set; }
        public List<Movie> movieList { get; set; }
         public List<Screen> screenlist { get; set; }
        public Screen screen { get; set; }
    }
}

