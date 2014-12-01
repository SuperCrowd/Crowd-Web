namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserJobApplication")]
    public partial class UserJobApplication
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long UserID { get; set; }

        public long JobID { get; set; }

        public virtual Job Job { get; set; }

        public virtual User User { get; set; }
    }
}
