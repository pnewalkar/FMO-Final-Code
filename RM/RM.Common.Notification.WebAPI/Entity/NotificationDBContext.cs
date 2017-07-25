namespace RM.Common.Notification.WebAPI.Entities
{
    using System.Data.Entity;

    public partial class NotificationDBContext : AuditContext
    {
        public NotificationDBContext()
            : base("name=NotificationDBContext")
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .Property(e => e.Notification_Heading)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Notification_Message)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.NotificationSource)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.PostcodeDistrict)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.PostcodeSector)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}