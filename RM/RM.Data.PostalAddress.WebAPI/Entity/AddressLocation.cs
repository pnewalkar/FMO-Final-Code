namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AddressLocation")]
    public partial class AddressLocation
    {
        public Guid ID { get; private set; }

        public int UDPRN { get; private set; }

        [Required]
        public DbGeometry LocationXY { get; private set; }

        [Column(TypeName = "numeric")]
        public decimal Lattitude { get; private set; }

        [Column(TypeName = "numeric")]
        public decimal Longitude { get; private set; }
    }
}
