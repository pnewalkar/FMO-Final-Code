using System.Data.Entity;

namespace RM.Common.ReferenceData.WebAPI.Entities
{
    /// <summary>
    /// Reference Data Database Context
    /// </summary>
    public class ReferenceDataDBContext : DbContext
    {
        public ReferenceDataDBContext()
            : base("name=ReferenceDataDBContext")
        {
        }

        public virtual DbSet<ReferenceData> ReferenceDatas { get; set; }

        public virtual DbSet<ReferenceDataCategory> ReferenceDataCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.ReferenceDataName)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.ReferenceDataValue)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.DataDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.DisplayText)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceDataCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceDataCategory>()
                .HasMany(e => e.ReferenceDatas)
                .WithRequired(e => e.ReferenceDataCategory)
                .HasForeignKey(e => e.ReferenceDataCategory_GUID)
                .WillCascadeOnDelete(false);
        }
    }
}