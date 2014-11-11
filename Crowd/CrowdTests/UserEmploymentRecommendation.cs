namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserEmploymentRecommendation")]
    public partial class UserEmploymentRecommendation
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Recommendation { get; set; }

        [Required]
        [StringLength(250)]
        public string RecommenderName { get; set; }

        public long UserID { get; set; }

        public virtual User User { get; set; }
    }
}
