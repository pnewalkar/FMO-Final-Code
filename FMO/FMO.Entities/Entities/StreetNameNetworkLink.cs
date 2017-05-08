namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.StreetNameNetworkLink")]
    public partial class StreetNameNetworkLink
    {
        public Guid ID { get; set; }

        public Guid? StreetName_GUID { get; set; }

        public Guid? NetworkLink_GUID { get; set; }

        [StringLength(12)]
        public string USRN { get; set; }

        [StringLength(20)]
        public string RoadLinkTOID { get; set; }
    }
}
