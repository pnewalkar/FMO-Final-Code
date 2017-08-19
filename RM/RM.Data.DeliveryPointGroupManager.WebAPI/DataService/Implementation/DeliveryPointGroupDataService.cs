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
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
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

            using (loggingHelper.RMTraceManager.StartTrace("Data.CreateDeliveryGroup"))
            {
                string methodName = typeof(DeliveryPointGroupDataService) + "." + nameof(CreateDeliveryGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointGroup != null && 
                    deliveryPointGroup.groupCentroidLocation.LocationRelationships != null && 
                    deliveryPointGroup.groupCentroidLocation.LocationRelationships1 != null)
                {
                    ConfigureMapper();

                    DataContext.Locations.Add(Mapper.Map<LocationDataDTO, Location>(deliveryPointGroup.groupBoundaryLocation));
                    DataContext.Locations.Add(Mapper.Map<LocationDataDTO, Location>(deliveryPointGroup.groupCentroidLocation));
                    DataContext.DeliveryPoints.Add(Mapper.Map<DeliveryPointDataDTO, DeliveryPoint>(deliveryPointGroup.groupCentroidDeliveryPoint));
                    DataContext.NetworkNodes.Add(Mapper.Map<NetworkNodeDataDTO, NetworkNode>(deliveryPointGroup.groupCentroidNetworkNode));
                    DataContext.SupportingDeliveryPoint.Add(Mapper.Map<SupportingDeliveryPointDataDTO, SupportingDeliveryPoint>(deliveryPointGroup.groupDetails));

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

        public DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"DataService.{nameof(UpdateDeliveryGroup)}"))
            {
                //fetch all delivery point locations
                var existingDeliveryLocation = DataContext.LocationRelationships.Where(x => x.RelatedLocationID == deliveryPointGroupDto.ID);
            }

            return deliveryPointGroupDto;
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

        #endregion PublicMethods

        #region PrivateMethods

        /// <summary>
        /// Automapper to convert DataDto to Entity
        /// </summary>
        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Location, LocationDataDTO>().ReverseMap();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>().ReverseMap();
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>().ReverseMap();
                cfg.CreateMap<DeliveryPointStatus, DeliveryPointStatusDataDTO>().ReverseMap();
                cfg.CreateMap<LocationRelationship, LocationRelationshipDataDTO>().ReverseMap();
                cfg.CreateMap<LocationOffering, LocationOfferingDataDTO>().ReverseMap();
                cfg.CreateMap<SupportingDeliveryPoint, SupportingDeliveryPointDataDTO>().ReverseMap();                
            });

            Mapper.Configuration.CreateMapper();
        }

        #endregion PrivateMethods
    }
}