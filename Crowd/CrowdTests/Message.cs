namespace CrowdTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        public long ID { get; set; }

        public DateTime DateCreated { get; set; }

        public long SenderID { get; set; }

        public long ReceiverID { get; set; }

        public bool State { get; set; }

        [Column("Message", TypeName = "text")]
        public string Message1 { get; set; }

        public string LinkURL { get; set; }

        public long? LinkJobID { get; set; }

        public long? LinkUserID { get; set; }

        public long MessageTypeID { get; set; }

        public virtual Job Job { get; set; }

        public virtual MessageType MessageType { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
