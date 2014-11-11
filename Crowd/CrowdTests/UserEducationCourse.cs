namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserEducationCourse")]
    public partial class UserEducationCourse
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long EducationID { get; set; }

        [Required]
        [StringLength(250)]
        public string Course { get; set; }

        public virtual UserEducation UserEducation { get; set; }
    }
}
