namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Follow")]
    public partial class Follow
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long FollowerUserID { get; set; }

        public long FollowingUserID { get; set; }


        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public Follow() { }
        public Follow(User follower, User following)
        {
            this.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            this.User = follower;
            this.User1 = following;
        }
    }
}
