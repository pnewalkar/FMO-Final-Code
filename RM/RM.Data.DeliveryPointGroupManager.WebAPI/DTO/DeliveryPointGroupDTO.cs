using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DTO
{
    public class DeliveryPointGroupDTO
    {
        public DeliveryPointGroupDTO()
        {
            this.AddedDeliveryPoints = new HashSet<DeliveryPointDTO>();
        }

        public Guid ID { get; set; }

        public string GroupName { get; set; }

        public byte? NumberOfFloors { get; set; }

        public double? InternalDistanceMeters { get; set; }

        public double? WorkloadTimeOverrideMinutes { get; set; }

        public string TimeOverrideReason { get; set; }

        public bool? TimeOverrideApproved { get; set; }

        public Guid? GroupTypeGUID { get; set; }

        public Guid? ServicePointTypeGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public bool? DeliverToReception { get; set; }

        public Guid NetworkNodeType { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public DbGeometry GroupCentroid { get; set; }

        public DbGeometry GroupBoundary { get; set; }

        public Guid LocationRelationshipForCentroidToBoundaryGuid { get; set; }

        public Guid RelationshipTypeForCentroidToBoundaryGUID { get; set; }

        public Guid RelationshipTypeForCentroidToDeliveryPointGUID { get; set; }

        public Guid GroupBoundaryGUID { get; set; }

        public ICollection<DeliveryPointDTO> AddedDeliveryPoints { get; set; }

        public Guid PolygonLocationId { get; set; }

        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry GroupPolygon { get; set; }
    }
}