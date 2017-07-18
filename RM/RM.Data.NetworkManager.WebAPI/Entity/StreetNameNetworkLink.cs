namespace RM.DataManagement.NetworkManager.WebAPI.Entities
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

        public Guid? StreetNameID { get; set; }

        public Guid? NetworkLinkID { get; set; }

        [StringLength(12)]
        public string USRN { get; set; }

        [StringLength(20)]
        public string RoadLinkTOID { get; set; }
    }
}
