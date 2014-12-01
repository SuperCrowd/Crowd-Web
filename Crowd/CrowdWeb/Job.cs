namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Job")]
    public partial class Job
    {
        public Job()
        {
            Feeds = new HashSet<Feed>();
            JobSkills = new HashSet<JobSkill>();
            Messages = new HashSet<Message>();
            UserJobApplications = new HashSet<UserJobApplication>();
            UserJobFavorites = new HashSet<UserJobFavorite>();
        }

        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public long UserID { get; set; }

        [Required]
        [StringLength(250)]
        public string Company { get; set; }

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

        [Required]
        [StringLength(250)]
        public string Industry2 { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Responsibilities { get; set; }

        [Column(TypeName = "text")]
        public string Qualifications { get; set; }

        [Column(TypeName = "text")]
        public string EmployerIntroduction { get; set; }

        public string URL { get; set; }

        public string ShareURL { get; set; }

        public bool State { get; set; }

        public long? ExperienceLevelType { get; set; }

        public virtual ExperienceLevelType ExperienceLevelType1 { get; set; }

        public virtual ICollection<Feed> Feeds { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<JobSkill> JobSkills { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<UserJobApplication> UserJobApplications { get; set; }

        public virtual ICollection<UserJobFavorite> UserJobFavorites { get; set; }
    }
}
