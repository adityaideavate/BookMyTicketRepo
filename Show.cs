//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookMyTicket
{
    using System;
    using System.Collections.Generic;
    
    public partial class Show
    {
        public int ShowId { get; set; }
        public int MovieId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBY { get; set; }
        public int ScreenId { get; set; }
        public int Rate { get; set; }
        public System.TimeSpan Time { get; set; }


        public virtual Movie Movie { get; set; }
        public virtual Screen Screen { get; set; }
    }
}