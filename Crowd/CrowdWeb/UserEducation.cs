namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserEducation")]
    public partial class UserEducation
    {
        public UserEducation()
        {
            UserEducationCourses = new HashSet<UserEducationCourse>();
        }

        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public long UserID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Degree { get; set; }

        public int StartYear { get; set; }

        public int? EndYear { get; set; }

        public int StartMonth { get; set; }

        public int? EndMonth { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<UserEducationCourse> UserEducationCourses { get; set; }
    }
}
