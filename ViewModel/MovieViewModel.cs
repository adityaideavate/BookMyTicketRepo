using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyTicket.ViewModel
{
    public class MovieViewModel
    {
        public Movie movie { get; set; }
        public List<Language> languagelist { get; set; }
        public List<Genre> genrelist { get; set; }
    }
}