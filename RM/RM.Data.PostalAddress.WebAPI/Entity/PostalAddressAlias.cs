namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.PostalAddressAlias")]
    public partial class PostalAddressAlias
    {
        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public Guid AliasTypeGUID { get; set; }

        public Guid? PostalAddressIdentifierID { get; set; }

        [StringLength(100)]
        public string AliasName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PreferenceOrderIndex { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime RowCreateDateTime { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }
    }
}