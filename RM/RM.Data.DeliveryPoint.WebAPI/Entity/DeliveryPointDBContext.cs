namespace RM.Data.DeliveryPoint.WebAPI.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DeliveryPointDBContext : DbContext
    {
        public DeliveryPointDBContext()
            : base("name=DeliveryPointDBContext")
        {
        }

        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public virtual DbSet<DeliveryPointAlias> DeliveryPointAlias { get; set; }
        public virtual DbSet<DeliveryPointStatus> DeliveryPointStatus { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }
        public virtual DbSet<RMGDeliveryPoint> RMGDeliveryPoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<DeliveryPoint>()
                .HasMany(e => e.DeliveryPointStatus)
                .WithRequired(e => e.DeliveryPoint)
                .HasForeignKey(e => e.LocationGUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryPointAlias>()
                .Property(e => e.DPAlias)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasOptional(e => e.NetworkNode)
                .WithRequired(e => e.Location);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.DeliveryPoint)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<RMGDeliveryPoint>()
                .Property(e => e.Latitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<RMGDeliveryPoint>()
                .Property(e => e.Longitude)
                .HasPrecision(38, 8);
        }
    }
}
