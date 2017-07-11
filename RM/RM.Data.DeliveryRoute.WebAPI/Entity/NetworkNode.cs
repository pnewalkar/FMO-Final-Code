using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.NetworkNode")]
    public partial class NetworkNode
    {
        public Guid ID { get; private set; }

        public Guid NetworkNodeType_GUID { get; private set; }

        [StringLength(20)]
        public string TOID { get; private set; }

        public Guid? DataProviderGUID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        public virtual DeliveryPoint DeliveryPoint { get; private set; }

        public virtual Location Location { get; private set; }
    }
}