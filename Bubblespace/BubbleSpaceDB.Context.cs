﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BubbleSpaceDb : DbContext
    {
        public BubbleSpaceDb()
            : base("name=BubbleSpaceDb")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<bubble_errors> bubble_errors { get; set; }
        public virtual DbSet<bubble_groups> bubble_groups { get; set; }
        public virtual DbSet<chat_members> chat_members { get; set; }
        public virtual DbSet<chats> chats { get; set; }
        public virtual DbSet<event_users> event_users { get; set; }
        public virtual DbSet<events> events { get; set; }
        public virtual DbSet<friends_added> friends_added { get; set; }
        public virtual DbSet<group_users> group_users { get; set; }
        public virtual DbSet<like_comments> like_comments { get; set; }
        public virtual DbSet<messages> messages { get; set; }
        public virtual DbSet<post_comments> post_comments { get; set; }
        public virtual DbSet<post_likes> post_likes { get; set; }
        public virtual DbSet<posts> posts { get; set; }
        public virtual DbSet<user_ranks> user_ranks { get; set; }
    }
}
