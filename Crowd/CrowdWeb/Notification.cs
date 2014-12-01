namespace CrowdWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notification")]
    public partial class Notification
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateSent { get; set; }

        public bool HasSent { get; set; }

        public string Error { get; set; }

        [Required]
        public string PushMessage { get; set; }

        [Required]
        public string DeviceToken { get; set; }

        [StringLength(100)]
        public string SourceTable { get; set; }

        [StringLength(100)]
        public string UserID { get; set; }

        [StringLength(100)]
        public string JobID { get; set; }
    }
}
