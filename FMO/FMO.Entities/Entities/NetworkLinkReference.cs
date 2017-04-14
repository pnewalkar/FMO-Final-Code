namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkLinkReference")]
    public partial class NetworkLinkReference
    {
        public int NetworkReference_Id { get; set; }

        public int NetworkLink_Id { get; set; }

        [StringLength(20)]
        public string OSRoadLinkTOID { get; set; }

        public Guid ID { get; set; }

        public Guid? OSRoadLink_GUID { get; set; }

        public Guid NetworkReference_GUID { get; set; }

        public Guid NetworkLink_GUID { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }

        public virtual OSRoadLink OSRoadLink { get; set; }
    }
}
