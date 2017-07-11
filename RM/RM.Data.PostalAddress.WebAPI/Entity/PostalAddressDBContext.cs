namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System.Data.Entity;

    public partial class PostalAddressDBContext : AuditContext
    {
        public PostalAddressDBContext()
            : base("name=PostalAddressDBContext")
        {
        }

        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }
        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }
        public virtual DbSet<PostalAddressStatus> PostalAddressStatus { get; set; }
        public virtual DbSet<FileProcessingLog> FileProcessingLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasOptional(e => e.NetworkNode)
                .WithRequired(e => e.Location);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.PostcodeType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.OrganisationName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.BuildingName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.SubBuildingName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.Thoroughfare)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DependentThoroughfare)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DependentLocality)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DoubleDependentLocality)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.PostTown)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.Postcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DeliveryPointSuffix)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.SmallUserOrganisationIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.POBoxNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.PostalAddressStatus)
                .WithRequired(e => e.PostalAddress)
                .HasForeignKey(e => e.PostalAddressGUID)
                .WillCascadeOnDelete(false);
        }
    }
}