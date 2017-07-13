using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.NetworkNode")]
    public partial class NetworkNode
    {
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