namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities
{
    using System.Data.Entity;

    public partial class DeliveryPointGroupDBContext : DbContext
    {
        public DeliveryPointGroupDBContext()
            : base("name=DeliveryPointGroupDBContext")
        {
        }

        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public virtual DbSet<DeliveryPointStatus> DeliveryPointStatus { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationOffering> LocationOfferings { get; set; }
        public virtual DbSet<LocationRelationship> LocationRelationships { get; set; }
        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }
        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }

        public virtual DbSet<SupportingDeliveryPoint> SupportingDeliveryPoint { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<DeliveryPoint>()
                .HasMany(e => e.DeliveryPointStatus)
                .WithRequired(e => e.DeliveryPoint)
                .HasForeignKey(e => e.LocationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryPoint>()
              .HasOptional(e => e.SupportingDeliveryPoint)
              .WithRequired(e => e.DeliveryPoint);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationOfferings)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationRelationships)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationRelationships1)
                .WithRequired(e => e.Location1)
                .HasForeignKey(e => e.RelatedLocationID)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<SupportingDeliveryPoint>()
               .Property(e => e.GroupName)
               .IsUnicode(false);

            modelBuilder.Entity<SupportingDeliveryPoint>()
                .Property(e => e.TimeOverrideReason)
                .IsUnicode(false);
        }
    }
}