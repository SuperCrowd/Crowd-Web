namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feed")]
    public partial class Feed
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long UserID { get; set; }

        public long FeedTypeID { get; set; }

        public long? JobID { get; set; }

        public long? OtherUserID { get; set; }

        public virtual FeedType FeedType { get; set; }

        public virtual Job Job { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
