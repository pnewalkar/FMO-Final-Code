using System.Data.Entity.Spatial;

namespace Fmo.DTO
{
    /// <summary>
    /// This class represents data transfer object for AccessLink entity
    /// </summary>
    public class AccessLinkDTO
    {
        public int AccessLink_Id { get; set; }

        public DbGeometry OperationalObjectPoint { get; set; }

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

        public object features { get; set; }
    }
}