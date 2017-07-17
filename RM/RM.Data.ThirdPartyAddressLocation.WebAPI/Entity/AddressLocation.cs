namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AddressLocation")]
    public partial class AddressLocation
    {
        public Guid ID { get; set; }

        public int UDPRN { get; set; }

        [Required]
        public DbGeometry LocationXY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Lattitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Longitude { get; set; }
    }
}
