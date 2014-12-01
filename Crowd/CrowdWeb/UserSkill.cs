namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserSkill")]
    public partial class UserSkill
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long UserID { get; set; }

        [Required]
        [StringLength(250)]
        public string Skill { get; set; }

        public virtual User User { get; set; }
    }
}
