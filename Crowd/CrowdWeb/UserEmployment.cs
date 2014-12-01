namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserEmployment")]
    public partial class UserEmployment
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public long UserID { get; set; }

        [Required]
        [StringLength(250)]
        public string EmployerName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(250)]
        public string LocationCity { get; set; }

        [StringLength(250)]
        public string LocationState { get; set; }

        [Required]
        [StringLength(250)]
        public string LocationCountry { get; set; }

        public int StartYear { get; set; }

        public int? EndYear { get; set; }

        [Column(TypeName = "text")]
        public string Summary { get; set; }

        public int StartMonth { get; set; }

        public int? EndMonth { get; set; }

        public virtual User User { get; set; }
    }
}
