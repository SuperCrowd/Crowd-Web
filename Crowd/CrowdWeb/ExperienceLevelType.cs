namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExperienceLevelType")]
    public partial class ExperienceLevelType
    {
        public ExperienceLevelType()
        {
            Jobs = new HashSet<Job>();
            Users = new HashSet<User>();
        }

        public long ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
