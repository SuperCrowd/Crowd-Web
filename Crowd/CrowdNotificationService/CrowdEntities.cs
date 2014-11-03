namespace CrowdNotificationService
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CrowdEntities : DbContext
    {
        public CrowdEntities()
            : base("name=CrowdEntities")
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
