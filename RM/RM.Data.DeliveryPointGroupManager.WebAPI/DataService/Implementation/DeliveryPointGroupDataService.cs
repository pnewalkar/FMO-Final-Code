using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

//using AutoMapper;
using System.Linq;
using AutoMapper;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    public class DeliveryPointGroupDataService : DataServiceBase<SupportingDeliveryPoint, DeliveryPointGroupDBContext>, IDeliveryPointGroupDataService
    {
        private const int BNGCOORDINATESYSTEM = 27700;
        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupDataServiceMethodExitEventId;

        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initialises new instance of <see cref="DeliveryPointGroupDataService"/> that contains data methods related to delivery point group.
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="loggingHelper">Helper class for logging related functions.</param>
        public DeliveryPointGroupDataService(IDatabaseFactory<DeliveryPointGroupDBContext> databaseFactory, ILoggingHelper loggingHelper)
        : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #region PublicMethods

        /// <summary>
        /// This method is used to insert delivery point group.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>Unique identifier of delivery point.</returns>
        public bool CreateDeliveryGroup(DeliveryPointGroupDataDTO deliveryPointGroup)
        {
            bool isDeliveryPointGroupCreationSuccess = false;

            using (loggingHelper.RMTraceManager.StartTrace($"DataService.{nameof(CreateDeliveryGroup)}"))
            {
                string methodName = typeof(DeliveryPointGroupDataService) + "." + nameof(CreateDeliveryGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointGroup != null && deliveryPointGroup.AddedDeliveryPoints != null)
                {
                    Location groupShape = new Location();
                    groupShape.Shape = deliveryPointGroup.GroupBoundary.Shape;
                    groupShape.ID = deliveryPointGroup.GroupBoundary.ID;
                    groupShape.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.Locations.Add(groupShape);

                    Location groupCentroidLocation = new Location();
                    groupCentroidLocation.Shape = groupShape.Shape.Centroid;
                    groupCentroidLocation.ID = deliveryPointGroup.DeliveryGroup.ID;
                    groupCentroidLocation.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.Locations.Add(groupCentroidLocation);

                    NetworkNode groupCentroidNetworkNode = new NetworkNode();
                    groupCentroidNetworkNode.ID = deliveryPointGroup.DeliveryGroup.ID;
                    groupCentroidNetworkNode.RowCreateDateTime = DateTime.UtcNow;
                    groupCentroidNetworkNode.NetworkNodeType_GUID = deliveryPointGroup.NetworkNodeType;
                    DataContext.NetworkNodes.Add(groupCentroidNetworkNode);

                    DeliveryPoint groupCentroidDeliveryPoint = new DeliveryPoint();
                    groupCentroidDeliveryPoint.DeliveryPointUseIndicatorGUID = deliveryPointGroup.DeliveryPointUseIndicatorGUID;
                    groupCentroidDeliveryPoint.ID = deliveryPointGroup.DeliveryGroup.ID;
                    groupCentroidDeliveryPoint.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.DeliveryPoints.Add(groupCentroidDeliveryPoint);

                    DeliveryPointStatus groupCentroidDeliveryPointStatus = new DeliveryPointStatus();
                    groupCentroidDeliveryPointStatus.ID = Guid.NewGuid();
                    groupCentroidDeliveryPointStatus.LocationID = deliveryPointGroup.DeliveryGroup.ID;
                    groupCentroidDeliveryPointStatus.DeliveryPointStatusGUID = deliveryPointGroup.DeliveryGroupStatusGUID;
                    groupCentroidDeliveryPointStatus.StartDateTime = DateTime.UtcNow;
                    groupCentroidDeliveryPointStatus.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.DeliveryPoints.Add(groupCentroidDeliveryPoint);

                    LocationRelationship groupShapeToCentroidRelationship = new LocationRelationship();
                    groupShapeToCentroidRelationship.ID = Guid.NewGuid();
                    groupShapeToCentroidRelationship.LocationID = deliveryPointGroup.DeliveryGroup.ID;
                    groupShapeToCentroidRelationship.RelatedLocationID = deliveryPointGroup.GroupBoundary.ID;
                    groupShapeToCentroidRelationship.RelationshipTypeGUID = deliveryPointGroup.RelationshipTypeForCentroidToBoundaryGUID;
                    groupShapeToCentroidRelationship.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.LocationRelationships.Add(groupShapeToCentroidRelationship);

                    foreach (var deliveryPoint in deliveryPointGroup.AddedDeliveryPoints)
                    {
                        LocationRelationship deliveryPointToCentroidRelation = new LocationRelationship();
                        deliveryPointToCentroidRelation.ID = Guid.NewGuid();
                        deliveryPointToCentroidRelation.LocationID = deliveryPoint.ID;
                        deliveryPointToCentroidRelation.RelatedLocationID = deliveryPointGroup.DeliveryGroup.ID;
                        deliveryPointToCentroidRelation.RelationshipTypeGUID = deliveryPointGroup.RelationshipTypeForCentroidToDeliveryPointGUID;
                        deliveryPointToCentroidRelation.RowCreateDateTime = DateTime.UtcNow;
                        DataContext.LocationRelationships.Add(deliveryPointToCentroidRelation);
                    }

                    SupportingDeliveryPoint groupDetails = new SupportingDeliveryPoint();
                    groupDetails.ID = deliveryPointGroup.DeliveryGroup.ID;
                    groupDetails.GroupName = deliveryPointGroup.DeliveryGroup.GroupName;
                    groupDetails.DeliverToReception = deliveryPointGroup.DeliveryGroup.DeliverToReception;
                    groupDetails.GroupTypeGUID = deliveryPointGroup.DeliveryGroup.GroupTypeGUID;
                    groupDetails.NumberOfFloors = deliveryPointGroup.DeliveryGroup.NumberOfFloors;
                    groupDetails.InternalDistanceMeters = deliveryPointGroup.DeliveryGroup.InternalDistanceMeters;
                    groupDetails.DeliverToReception = deliveryPointGroup.DeliveryGroup.DeliverToReception;
                    groupDetails.WorkloadTimeOverrideMinutes = deliveryPointGroup.DeliveryGroup.WorkloadTimeOverrideMinutes;
                    groupDetails.TimeOverrideApproved = deliveryPointGroup.DeliveryGroup.TimeOverrideApproved;
                    groupDetails.TimeOverrideReason = deliveryPointGroup.DeliveryGroup.TimeOverrideReason;
                    groupDetails.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.SupportingDeliveryPoint.Add(groupDetails);

                    DataContext.SaveChanges();

                    isDeliveryPointGroupCreationSuccess = true;
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isDeliveryPointGroupCreationSuccess;
            }
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Access Link Dto</returns>
        public List<DeliveryPointGroupDataDTO> GetDeliveryGroups(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<DeliveryPointGroupDataDTO> deliveryPointGroupdata = new List<DeliveryPointGroupDataDTO>();
            using (loggingHelper.RMTraceManager.StartTrace($"DataService.{nameof(GetDeliveryGroups)}"))
            {
                string methodName = typeof(DeliveryPointGroupDataService) + "." + nameof(GetDeliveryGroups);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var resultValue = GetDeliveryGroupCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
            }
            return deliveryPointGroupdata;
        }

        public DeliveryPointGroupDataDTO UpdateDeliveryGroup(DeliveryPointGroupDataDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"DataService.{nameof(UpdateDeliveryGroup)}"))
            {
                //fetch all delivery point locations
                var existingDeliveryPointLocationRelationships = DataContext.LocationRelationships.Where(x => x.RelatedLocationID == deliveryPointGroupDto.DeliveryGroup.DeliveryPoint.ID);
                var deliveryPointToCentroidRelationTypeId = existingDeliveryPointLocationRelationships.First().RelationshipTypeGUID;
                DataContext.LocationRelationships.RemoveRange(existingDeliveryPointLocationRelationships);

                foreach (var addedDeliveryPoint in deliveryPointGroupDto.AddedDeliveryPoints)
                {
                    LocationRelationship deliveryPointToCentroidRelation = new LocationRelationship();
                    deliveryPointToCentroidRelation.ID = Guid.NewGuid();
                    deliveryPointToCentroidRelation.LocationID = addedDeliveryPoint.ID;
                    deliveryPointToCentroidRelation.RelatedLocationID = deliveryPointGroupDto.DeliveryGroup.DeliveryPoint.ID;
                    deliveryPointToCentroidRelation.RelationshipTypeGUID = deliveryPointToCentroidRelationTypeId;
                    deliveryPointToCentroidRelation.RowCreateDateTime = DateTime.UtcNow;
                    DataContext.LocationRelationships.Add(deliveryPointToCentroidRelation);
                }

                // Update group boundary
                var groupBoundary = DataContext.Locations.Single(x => x.ID == deliveryPointGroupDto.GroupBoundary.ID);

                groupBoundary.Shape = deliveryPointGroupDto.GroupBoundary.Shape;

                // Update group details
                var existingGroupDetails = DataContext.SupportingDeliveryPoint.Single(x => x.ID == deliveryPointGroupDto.DeliveryGroup.ID);
                existingGroupDetails.GroupName = deliveryPointGroupDto.DeliveryGroup.GroupName;
                existingGroupDetails.DeliverToReception = deliveryPointGroupDto.DeliveryGroup.DeliverToReception;
                existingGroupDetails.GroupTypeGUID = deliveryPointGroupDto.DeliveryGroup.GroupTypeGUID;
                existingGroupDetails.NumberOfFloors = deliveryPointGroupDto.DeliveryGroup.NumberOfFloors;
                existingGroupDetails.InternalDistanceMeters = deliveryPointGroupDto.DeliveryGroup.InternalDistanceMeters;

                existingGroupDetails.WorkloadTimeOverrideMinutes = deliveryPointGroupDto.DeliveryGroup.WorkloadTimeOverrideMinutes;
                existingGroupDetails.TimeOverrideApproved = deliveryPointGroupDto.DeliveryGroup.TimeOverrideApproved;
                existingGroupDetails.TimeOverrideReason = deliveryPointGroupDto.DeliveryGroup.TimeOverrideReason;
                existingGroupDetails.ServicePointTypeGUID = deliveryPointGroupDto.DeliveryGroup.ServicePointTypeGUID;
                existingGroupDetails.RowCreateDateTime = DateTime.UtcNow;

                //update group centroid
                existingGroupDetails.DeliveryPoint.NetworkNode.Location.Shape = deliveryPointGroupDto.GroupBoundary.Shape.Centroid;

                DataContext.SaveChanges();
            }

            return deliveryPointGroupDto;
        }

        public DeliveryPointGroupDataDTO GetDeliveryGroup(Guid deliveryGroupId)
        {
            DeliveryPointGroupDataDTO deliveryGroups = new DeliveryPointGroupDataDTO();

            var groupDetails = (from location in DataContext.Locations
                                from groupDetail in DataContext.SupportingDeliveryPoint
                                from locationRelation in DataContext.LocationRelationships
                                where location.ID == locationRelation.RelatedLocationID
                                && groupDetail.DeliveryPoint.ID == locationRelation.LocationID
                                && groupDetail.ID == deliveryGroupId
                                select new
                                {
                                    Location = location,
                                    GroupDetail = groupDetail,
                                    AddedDeliveryPoints = (from addedDeliveryPoints in DataContext.Locations
                                                           from groupDPLocationRelationships in DataContext.LocationRelationships
                                                           where addedDeliveryPoints.ID == groupDPLocationRelationships.LocationID
                                                           && groupDPLocationRelationships.RelatedLocationID == groupDetail.DeliveryPoint.ID
                                                           select addedDeliveryPoints).AsEnumerable()
                                }
                               ).Single();

            ConfigureMapper();

            deliveryGroups = Mapper.Map<DeliveryPointGroupDataDTO>(groupDetails);

            return deliveryGroups;
        }

        #endregion PublicMethods

        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SupportingDeliveryPoint, SupportingDeliveryPointDataDTO>().ReverseMap();
                cfg.CreateMap<Location, LocationDataDTO>().ReverseMap();
                cfg.CreateMap<LocationOffering, LocationOfferingDataDTO>().ReverseMap();
                cfg.CreateMap<LocationRelationship, LocationRelationshipDataDTO>().ReverseMap();
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>().ReverseMap();
            });

            Mapper.Configuration.CreateMapper();
        }

        private IEnumerable<DeliveryPointGroupDataDTO> GetDeliveryGroupCoordinatesDataByBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            IEnumerable<DeliveryPointGroupDataDTO> deliveryGroups = null;
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.Shape).SingleOrDefault();

                DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                var groupDetails = from location in DataContext.Locations
                                   from groupDetail in DataContext.SupportingDeliveryPoint
                                   from locationRelation in DataContext.LocationRelationships
                                   where location.ID == locationRelation.RelatedLocationID
                                   && groupDetail.DeliveryPoint.ID == locationRelation.LocationID
                                   && location.Shape.Intersects(extent)
                                   && location.Shape.Intersects(polygon)
                                   select new
                                   {
                                       Location = location,
                                       GroupDetail = groupDetail,
                                       AddedDeliveryPoints = (from addedDeliveryPoints in DataContext.Locations
                                                              from groupDPLocationRelationships in DataContext.LocationRelationships
                                                              where addedDeliveryPoints.ID == groupDPLocationRelationships.LocationID
                                                              && groupDPLocationRelationships.RelatedLocationID == groupDetail.DeliveryPoint.ID
                                                              select addedDeliveryPoints).AsEnumerable()
                                   };

                ConfigureMapper();

                deliveryGroups = groupDetails.Select(Mapper.Map<DeliveryPointGroupDataDTO>);

                return deliveryGroups;
            }

            return deliveryGroups;
        }

        #region PrivateMethods

        /// <summary>
        /// Automapper to convert DataDto to Entity
        /// </summary>
        //private static void ConfigureMapper()
        //{
        //    Mapper.Initialize(cfg =>
        //    {
        //        cfg.CreateMap<Location, LocationDataDTO>().ReverseMap();
        //        cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>().ReverseMap();
        //        cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>().ReverseMap();
        //        cfg.CreateMap<DeliveryPointStatus, DeliveryPointStatusDataDTO>().ReverseMap();
        //        cfg.CreateMap<LocationRelationship, LocationRelationshipDataDTO>().ReverseMap();
        //        cfg.CreateMap<LocationOffering, LocationOfferingDataDTO>().ReverseMap();
        //        cfg.CreateMap<SupportingDeliveryPoint, SupportingDeliveryPointDataDTO>().ReverseMap();
        //    });

        //    Mapper.Configuration.CreateMapper();
        //}

        #endregion PrivateMethods
    }
}