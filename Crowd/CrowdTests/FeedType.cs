namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedType")]
    public partial class FeedType
    {
        public FeedType()
        {
            Feeds = new HashSet<Feed>();
        }

        public long ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        public virtual ICollection<Feed> Feeds { get; set; }
    }
}
