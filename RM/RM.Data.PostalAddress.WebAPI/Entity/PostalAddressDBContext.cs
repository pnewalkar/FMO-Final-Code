namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PostalAddressDBContext : DbContext
    {
        public PostalAddressDBContext()
            : base("name=PostalAddressDBContext")
        {
        }

        public virtual DbSet<AddressLocation> AddressLocations { get; set; }
        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public virtual DbSet<FileProcessingLog> FileProcessingLogs { get; set; }
        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }
        public virtual DbSet<PostalAddressStatus> PostalAddressStatus { get; set; }
        public virtual DbSet<Postcode> Postcodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressLocation>()
                .Property(e => e.Lattitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<AddressLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<FileProcessingLog>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<FileProcessingLog>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<FileProcessingLog>()
                .Property(e => e.ErrorMessage)
                .IsUnicode(false);

            modelBuilder.Entity<FileProcessingLog>()
                .Property(e => e.AmendmentType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FileProcessingLog>()
                .Property(e => e.Comments)
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
                .HasMany(e => e.DeliveryPoints)
                .WithRequired(e => e.PostalAddress)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.PostalAddressStatus)
                .WithRequired(e => e.PostalAddress)
                .HasForeignKey(e => e.PostalAddressGUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.PostcodeUnit)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.OutwardCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.InwardCode)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
