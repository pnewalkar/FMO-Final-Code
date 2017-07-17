namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    using System;
    using System.Data.Entity.Spatial;    
    public class AddressLocationDataDTO
    {
        public Guid ID { get; set; }

        public int UDPRN { get; set; }

        public DbGeometry LocationXY { get; set; }

        public decimal Lattitude { get; set; }
                
        public decimal Longitude { get; set; }
    }
}
