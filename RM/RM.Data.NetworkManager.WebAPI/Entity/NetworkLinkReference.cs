namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkLinkReference")]
    public partial class NetworkLinkReference
    {
        public Guid ID { get; set; }

        [StringLength(20)]
        public string OSRoadLinkTOID { get; set; }

        public Guid? OSRoadLinkID { get; set; }

        public Guid NetworkReferenceID { get; set; }

        public Guid NetworkLinkID { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }

        public virtual OSRoadLink OSRoadLink { get; set; }
    }
}
