﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DAL.Entities;

namespace DAL.Concrete
{
    public class TagDbContext : DbContext
    {
        public TagDbContext() : base() { }
        public TagDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbSet<Param> Params { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagValue> TagValues { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        //public DbSet<vAnalogHistory> vAnalogHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Tag>().
                HasMany(c => c.Groups).
                WithMany(p => p.Tags).
                Map(
                m =>
                {
                    m.MapLeftKey("TagID");
                    m.MapRightKey("GroupID");
                    m.ToTable("TagGroups");
                });
        }
    }
}