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
    
    public partial class group_users
    {
        public int C_ID { get; set; }
        public Nullable<int> FK_group_users_bubble_group { get; set; }
        public string FK_group_users_users { get; set; }
        public Nullable<bool> group_admin { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual bubble_groups bubble_groups { get; set; }
    }
}
