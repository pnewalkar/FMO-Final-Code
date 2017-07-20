namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkNode")]
    public partial class NetworkNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NetworkNode()
        {
        }

        public Guid ID { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        [StringLength(20)]
        public string TOID { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }

        public virtual Location Location { get; set; }
    }
}
