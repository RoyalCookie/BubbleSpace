//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bubblespace
{
    using System;
    using System.Collections.Generic;
    
    public partial class events
    {
        public events()
        {
            this.event_users = new HashSet<event_users>();
        }
    
        public int C_ID { get; set; }
        public string event_name { get; set; }
        public string event_description { get; set; }
        public Nullable<System.DateTime> event_start_time { get; set; }
        public Nullable<System.DateTime> event_end_time { get; set; }
        public string FK_events_owner { get; set; }
        public string event_profile_image { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual ICollection<event_users> event_users { get; set; }
    }
}
