namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobSkill")]
    public partial class JobSkill
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long JobID { get; set; }

        [Required]
        [StringLength(250)]
        public string Skill { get; set; }

        public virtual Job Job { get; set; }
    }
}
