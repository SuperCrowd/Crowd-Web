namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            Feeds = new HashSet<Feed>();
            Feeds1 = new HashSet<Feed>();
            Follows = new HashSet<Follow>();
            Follows1 = new HashSet<Follow>();
            Jobs = new HashSet<Job>();
            Messages = new HashSet<Message>();
            Messages1 = new HashSet<Message>();
            Messages2 = new HashSet<Message>();
            UserEducations = new HashSet<UserEducation>();
            UserEmployments = new HashSet<UserEmployment>();
            UserEmploymentRecommendations = new HashSet<UserEmploymentRecommendation>();
            UserJobApplications = new HashSet<UserJobApplication>();
            UserJobFavorites = new HashSet<UserJobFavorite>();
            UserSkills = new HashSet<UserSkill>();
        }

        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        [StringLength(250)]
        public string LocationCity { get; set; }

        [StringLength(250)]
        public string LocationState { get; set; }

        [Required]
        [StringLength(250)]
        public string LocationCountry { get; set; }

        [Required]
        [StringLength(250)]
        public string Industry { get; set; }

        [StringLength(250)]
        public string Industry2 { get; set; }

        [Column(TypeName = "text")]
        public string Summary { get; set; }

        [StringLength(250)]
        public string PhotoURL { get; set; }

        [StringLength(250)]
        public string LinkedInId { get; set; }

        [StringLength(250)]
        public string DeviceToken { get; set; }

        public long ExperienceLevelType { get; set; }

        public string Token { get; set; }

        public DateTime? TokenExpireTime { get; set; }

        public virtual ExperienceLevelType ExperienceLevelType1 { get; set; }

        public virtual ICollection<Feed> Feeds { get; set; }

        public virtual ICollection<Feed> Feeds1 { get; set; }

        public virtual ICollection<Follow> Follows { get; set; }

        public virtual ICollection<Follow> Follows1 { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Message> Messages1 { get; set; }

        public virtual ICollection<Message> Messages2 { get; set; }

        public virtual ICollection<UserEducation> UserEducations { get; set; }

        public virtual ICollection<UserEmployment> UserEmployments { get; set; }

        public virtual ICollection<UserEmploymentRecommendation> UserEmploymentRecommendations { get; set; }

        public virtual ICollection<UserJobApplication> UserJobApplications { get; set; }

        public virtual ICollection<UserJobFavorite> UserJobFavorites { get; set; }

        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
