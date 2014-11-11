namespace CrowdTests
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CrowdEntities : DbContext
    {
        public CrowdEntities()
            : base("name=CrowdEntities")
        {
        }

        public virtual DbSet<ExperienceLevelType> ExperienceLevelTypes { get; set; }
        public virtual DbSet<Feed> Feeds { get; set; }
        public virtual DbSet<FeedType> FeedTypes { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobSkill> JobSkills { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserEducation> UserEducations { get; set; }
        public virtual DbSet<UserEducationCourse> UserEducationCourses { get; set; }
        public virtual DbSet<UserEmployment> UserEmployments { get; set; }
        public virtual DbSet<UserEmploymentRecommendation> UserEmploymentRecommendations { get; set; }
        public virtual DbSet<UserJobApplication> UserJobApplications { get; set; }
        public virtual DbSet<UserJobFavorite> UserJobFavorites { get; set; }
        public virtual DbSet<UserSkill> UserSkills { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExperienceLevelType>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.ExperienceLevelType1)
                .HasForeignKey(e => e.ExperienceLevelType);

            modelBuilder.Entity<ExperienceLevelType>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.ExperienceLevelType1)
                .HasForeignKey(e => e.ExperienceLevelType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeedType>()
                .HasMany(e => e.Feeds)
                .WithRequired(e => e.FeedType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.Responsibilities)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.Qualifications)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.EmployerIntroduction)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobSkills)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.Job)
                .HasForeignKey(e => e.LinkJobID);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.UserJobApplications)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.UserJobFavorites)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobSkill>()
                .Property(e => e.Skill)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.Message1)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.LinkURL)
                .IsUnicode(false);

            modelBuilder.Entity<MessageType>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.MessageType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Summary)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Token)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Feeds)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Feeds1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.OtherUserID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Follows)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.FollowerUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Follows1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.FollowingUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.SenderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.ReceiverID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages2)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.LinkUserID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserEducations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserEmployments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserEmploymentRecommendations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserJobApplications)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserJobFavorites)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserSkills)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserEducation>()
                .HasMany(e => e.UserEducationCourses)
                .WithRequired(e => e.UserEducation)
                .HasForeignKey(e => e.EducationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserEmployment>()
                .Property(e => e.Summary)
                .IsUnicode(false);

            modelBuilder.Entity<UserEmploymentRecommendation>()
                .Property(e => e.Recommendation)
                .IsUnicode(false);

            modelBuilder.Entity<UserSkill>()
                .Property(e => e.Skill)
                .IsUnicode(false);
        }
    }
}
