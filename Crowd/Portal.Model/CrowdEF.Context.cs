﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CrouwdEntities : DbContext
    {
        public CrouwdEntities()
            : base("name=CrouwdEntities")
        {
        }

        public CrouwdEntities(string Connection)
            : base(Connection)
        {
        }
    
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ExperienceLevelType> ExperienceLevelTypes { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedType> FeedTypes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<UserEducationCourse> UserEducationCourses { get; set; }
        public DbSet<UserEmployment> UserEmployments { get; set; }
        public DbSet<UserEmploymentRecommendation> UserEmploymentRecommendations { get; set; }
        public DbSet<UserJobApplication> UserJobApplications { get; set; }
        public DbSet<UserJobFavorite> UserJobFavorites { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
    }
}
