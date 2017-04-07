using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using System.Data.Entity.Spatial;

namespace Fmo.DTO
{
   public class AccessLinkDTO
    {

        public int AccessLink_Id { get; set; }

        public Geometry OperationalObjectPoint { get; set; }

        public DbGeometry NetworkIntersectionPoint { get; set; }

        public DbGeometry AccessLinkLine { get; set; }

        public int AccessLinkType_Id { get; set; }

        public bool? Approved { get; set; }

        public decimal ActualLengthMeter { get; set; }

        public decimal WorkloadLengthMeter { get; set; }

        public int LinkStatus_Id { get; set; }

        public int NetworkLink_Id { get; set; }

        public int LinkDirection_Id { get; set; }

        public int OperationalObjectId { get; set; }

        public int OperationalObjectType_Id { get; set; }

        public string type { get; set; }

        public List<Feature> features { get; set; }

        //public virtual DeliveryGroup DeliveryGroup { get; set; }

        //public virtual DeliveryPoint DeliveryPoint { get; set; }

        //public virtual NetworkLink NetworkLink { get; set; }

        //public virtual ReferenceData ReferenceData { get; set; }

        //public virtual ReferenceData ReferenceData1 { get; set; }

        //public virtual ReferenceData ReferenceData2 { get; set; }

        //public virtual ReferenceData ReferenceData3 { get; set; }

        //public virtual RMGDeliveryPoint RMGDeliveryPoint { get; set; }
    }
}
