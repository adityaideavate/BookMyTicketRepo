using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket
{
   

    [MetadataType(typeof(ShowMataData))]
    public partial class Show
    {
       
    }


    public class ShowMataData
    {
      

        [Range(70, 1000, ErrorMessage = " Rate cannot be less than 70 and more than 1000")]
            public int Rate { get; set; }


         

        [Range(typeof(TimeSpan), "08:00:00", "22:30:00", ErrorMessage = "Time must be between {1} and {2}")]
        public System.TimeSpan Time { get; set; }





    }








}