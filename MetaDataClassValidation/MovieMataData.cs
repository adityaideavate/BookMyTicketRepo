using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket
{
    


    [MetadataType(typeof(MovieMataData))]
    public partial class Movie
    {
        // public string Email { get; set; }
    }


    public class MovieMataData
    {
        [Required]
        public string MovieName { get; set; }

        [Required]
        public string MoviePic { get; set; }
    }

   

}