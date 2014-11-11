namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MessageType")]
    public partial class MessageType
    {
        public MessageType()
        {
            Messages = new HashSet<Message>();
        }

        public long ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
