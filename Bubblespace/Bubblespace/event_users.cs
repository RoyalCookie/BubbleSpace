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
    
    public partial class event_users
    {
        public int C_ID { get; set; }
        public string FK_event_users_users { get; set; }
        public Nullable<int> FK_event_users_events { get; set; }
        public Nullable<bool> event_admin { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual events events { get; set; }
    }
}