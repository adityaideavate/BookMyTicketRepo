using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMyTicket
{
    [MetadataType(typeof(ScreenMataData))]
    public partial class Screen
    {

    }

    public class ScreenMataData
    {
        [Range(50,300,ErrorMessage ="Seat capacity cannot be less than 50 or more than 300")]
        public int ScreenCapacity { get; set; }       
    }
}