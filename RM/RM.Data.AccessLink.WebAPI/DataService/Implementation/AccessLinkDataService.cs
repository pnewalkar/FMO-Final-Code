namespace RM.DataManagement.AccessLink.WebAPI.DataService.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using CommonLibrary.DataMiddleware;
    using CommonLibrary.ExceptionMiddleware;
    using CommonLibrary.HelperMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using Data.AccessLink.WebAPI.DataDTOs;
    using Data.AccessLink.WebAPI.Utils;
    using Entities;
    using Interfaces;

    /// <summary>
    /// This class contains methods of Access Link DataService for fetching Access Link data.
    /// </summary>
    public class AccessLinkDataService : DataServiceBase<AccessLink, AccessLinkDBContext>, IAccessLinkDataService
    {
        private const int BNGCOORDINATESYSTEM = 27700;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.AccessLinkAPIPriority;
        private int entryEventId = LoggerTraceConstants.AccessLinkDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.AccessLinkDataServiceMethodExitEventId;

        public AccessLinkDataService(IDatabaseFactory<AccessLinkDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Access Link Dto</returns>
        public List<AccessLinkDataDTO> GetAccessLinks(string boundingBoxCoordinates, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetAccessLinks"))
            {
                string methodName = typeof(AccessLinkDataService) + "." + nameof(GetAccessLinks);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var resultValue = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                ConfigureMapper();

                var accesslink = Mapper.Map<List<AccessLink>, List<AccessLinkDataDTO>>(resultValue);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return accesslink;
            }
        }

        /// <summary>
        /// Creates automatic access link.
        /// </summary>
        /// <param name="accessLinkDto">Access link data object.</param>
        /// <returns>Success.</returns>
        public bool CreateAccessLink(AccessLinkDataDTO accessLinkDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAutomaticAccessLink"))
                {
                    string methodName = typeof(AccessLinkDataService) + "." + nameof(CreateAccessLink);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    AccessLink entity = new AccessLink();
                    bool isAccessLinkCreationSuccess = false;

                    ConfigureMapper();

                    entity = Mapper.Map<AccessLinkDataDTO, AccessLink>(accessLinkDto);

                    DataContext.AccessLinks.Add(entity);

                    DataContext.SaveChanges();
                    isAccessLinkCreationSuccess = true;
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return isAccessLinkCreationSuccess;
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("automatic access link")));
            }
        }

        /// <summary>
        /// this method is used to get the access links crossing the created access link
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<AccessLinkDTO> </returns>
        public bool GetAccessLinksCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetAccessLinksCrossingOperationalObject"))
            {
                string methodName = typeof(AccessLinkDataService) + "." + nameof(GetAccessLinksCrossingOperationalObject);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                ConfigureMapper();
                bool isAccessLinkCrossing = default(bool);
                List<NetworkLinkDataDTO> accessLinkDataDTOs = new List<NetworkLinkDataDTO>();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                List<NetworkLink> crossingAccessLinks = DataContext.NetworkLinks.AsNoTracking().Where(al => al.LinkGeometry != null && al.LinkGeometry.Intersects(extent) && al.LinkGeometry.Crosses(accessLink)).ToList();
                List<NetworkLinkDataDTO> crossingAccessLinkDTOs = Mapper.Map<List<NetworkLink>, List<NetworkLinkDataDTO>>(crossingAccessLinks);
                accessLinkDataDTOs.AddRange(crossingAccessLinkDTOs);

                List<NetworkLink> overLappingAccessLinks = DataContext.NetworkLinks.AsNoTracking().Where(al => al.LinkGeometry != null && al.LinkGeometry.Intersects(extent) && al.LinkGeometry.Overlaps(accessLink)).ToList();
                List<NetworkLinkDataDTO> overLappingAccessLinkDTOs = Mapper.Map<List<NetworkLink>, List<NetworkLinkDataDTO>>(overLappingAccessLinks);
                accessLinkDataDTOs.AddRange(overLappingAccessLinkDTOs);

                if (accessLinkDataDTOs.Count > 0)
                {
                    isAccessLinkCrossing = false;
                }
                else
                {
                    isAccessLinkCrossing = true;
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isAccessLinkCrossing;
            }
        }

        /// <summary>
        /// Getting count of Deliverypoint intersecs with other DP.
        /// </summary>
        /// <param name="operationalObjectPoint"></param>
        /// <param name="accessLink"></param>
        /// <returns>integer count</returns>
        public int GetIntersectionCountForDeliveryPoint(DbGeometry operationalObjectPoint, DbGeometry accessLink)
        {
            var intersectionCount = DataContext.DeliveryPoints.AsNoTracking()
                  .Count(m => m.NetworkNode.Location.Shape.Intersects(accessLink) && !m.NetworkNode.Location.Shape.SpatialEquals(operationalObjectPoint));

            return intersectionCount;
        }

        /// <summary>
        /// Getting count of accesslink crosses or overlaps with other accesslink
        /// </summary>
        /// <param name="operationalObjectPoint"></param>
        /// <param name="accessLink"></param>
        /// <returns>integer count</returns>
        public bool CheckAccessLinkCrossesorOverLaps(DbGeometry operationalObjectPoint, DbGeometry accessLink)
        {
            // var accesslinkCount = DataContext.NetworkLinks.AsNoTracking().Where(al => al.LinkGeometry != null && al.LinkGeometry.Intersects(accessLink) && al.LinkGeometry.Crosses(accessLink)).ToList();
            var isAccessLinkCountForCrossesorOverLaps = DataContext.NetworkLinks.AsNoTracking().Any(a => a.LinkGeometry.Intersects(accessLink)
                                                     && !a.LinkGeometry.Intersection(accessLink).SpatialEquals(a.NetworkNode.Location.Shape));

            return isAccessLinkCountForCrossesorOverLaps;
        }

        /// <summary> This method is used to get the delivery points crossing the operationalObject
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        public bool GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryPointsCrossingOperationalObject"))
            {
                string methodName = typeof(AccessLinkDataService) + "." + nameof(GetDeliveryPointsCrossingOperationalObject);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                ConfigureMapper();
                bool isDeliveryPointCrossing = default(bool);
                List<DeliveryPointDataDTO> deliveryPointDTOs = new List<DeliveryPointDataDTO>();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), AccessLinkConstants.BNGCOORDINATESYSTEM);

                List<DeliveryPoint> crossingDeliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.NetworkNode.Location.Shape != null && dp.NetworkNode.Location.Shape.Intersects(extent) && dp.NetworkNode.Location.Shape.Crosses(operationalObject)).ToList();
                List<DeliveryPointDataDTO> crossingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(crossingDeliveryPoints);
                deliveryPointDTOs.AddRange(crossingAccessLinkDTOs);

                List<DeliveryPoint> overLappingDeliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.NetworkNode.Location.Shape != null && dp.NetworkNode.Location.Shape.Intersects(extent) && dp.NetworkNode.Location.Shape.Overlaps(operationalObject)).ToList();
                List<DeliveryPointDataDTO> overLappingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(overLappingDeliveryPoints);
                deliveryPointDTOs.AddRange(overLappingAccessLinkDTOs);

                if (deliveryPointDTOs.Count > 0)
                {
                    isDeliveryPointCrossing = false;
                }
                else
                {
                    isDeliveryPointCrossing = true;
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isDeliveryPointCrossing;
            }
        }

        /// <summary> Get the Network Links crossing the operational Object for a given extent</summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">accesslink coordinate array</param>
        /// <returns>List<NetworkLinkDTO></returns>
        public bool GetCrossingNetworkLink(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetCrossingNetworkLink"))
            {
                string methodName = typeof(AccessLinkDataService) + "." + nameof(GetCrossingNetworkLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                ConfigureMapper();
                bool isNetworkLinkCrossing = default(bool);
                List<NetworkLinkDataDTO> networkLinkDTOs = new List<NetworkLinkDataDTO>();

                DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                List<NetworkLink> crossingNetworkLinks = DataContext.NetworkLinks.AsNoTracking().Where(nl => nl.LinkGeometry != null && nl.LinkGeometry.Intersects(extent) && nl.LinkGeometry.Crosses(accessLink)).ToList();
                List<NetworkLinkDataDTO> crossingNetworkLinkDTOs = Mapper.Map<List<NetworkLink>, List<NetworkLinkDataDTO>>(crossingNetworkLinks);
                networkLinkDTOs.AddRange(crossingNetworkLinkDTOs);

                List<NetworkLink> overLappingNetworkLinks = DataContext.NetworkLinks.AsNoTracking().Where(nl => nl.LinkGeometry != null && nl.LinkGeometry.Intersects(extent) && nl.LinkGeometry.Overlaps(accessLink)).ToList();
                List<NetworkLinkDataDTO> overLappingNetworkLinkDTOs = Mapper.Map<List<NetworkLink>, List<NetworkLinkDataDTO>>(overLappingNetworkLinks);
                networkLinkDTOs.AddRange(overLappingNetworkLinkDTOs);

                if (networkLinkDTOs.Count > 0)
                {
                    isNetworkLinkCrossing = false;
                }
                else
                {
                    isNetworkLinkCrossing = true;
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isNetworkLinkCrossing;
            }
        }

        /// <summary>
        /// Automapper to convert DataDto to Entity
        /// </summary>
        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AccessLink, AccessLinkDataDTO>().ReverseMap();
                cfg.CreateMap<NetworkLink, NetworkLinkDataDTO>().ReverseMap();
                cfg.CreateMap<AccessLinkStatus, AccessLinkStatusDataDTO>().ReverseMap();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>().ReverseMap();
                cfg.CreateMap<Location, LocationDataDTO>().ReverseMap();
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>().ReverseMap();
            });

            Mapper.Configuration.CreateMapper();
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>Link of Access Link Entity</returns>
        private IEnumerable<AccessLink> GetAccessLinkCoordinatesDataByBoundingBox(string boundingBoxCoordinates, Guid locationGuid)
        {
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == locationGuid).Select(x => x.Shape).SingleOrDefault();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);
                return DataContext.AccessLinks.Include(m => m.NetworkLink).AsNoTracking().Where(x => x.NetworkLink.LinkGeometry.Intersects(extent) && x.NetworkLink.LinkGeometry.Intersects(polygon)).ToList();
            }

            return null;
        }

        /// <summary>
        /// Deletes a access link when delivery point is deleted
        /// </summary>
        /// <param name="operationalObjectId">Operation object id unique identifier.</param>
        /// <returns>Success of delete operation.</returns>
        public async Task<bool> DeleteAccessLink(Guid operationalObjectId, Guid networkLinkTypeGUID, Guid accessLinkTypeGUID)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.DeleteAccessLink"))
                {
                    string methodName = typeof(AccessLinkDataService) + "." + nameof(DeleteAccessLink);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                    AccessLink accessLink = await DataContext.AccessLinks.Include(l => l.NetworkLink).Where(l => l.NetworkLink.StartNodeID == operationalObjectId && l.NetworkLink.NetworkLinkTypeGUID == networkLinkTypeGUID && l.AccessLinkTypeGUID == accessLinkTypeGUID && l.ID == l.NetworkLink.ID).SingleOrDefaultAsync();

                    if (accessLink != null)
                    {
                        if (accessLink.AccessLinkStatus != null)
                        {
                            DataContext.AccessLinkStatus.RemoveRange(accessLink.AccessLinkStatus);
                        }

                        if (accessLink.NetworkLink != null)
                        {
                            DataContext.NetworkLinks.Remove(accessLink.NetworkLink);
                        }

                        DataContext.AccessLinks.Remove(accessLink);

                        await DataContext.SaveChangesAsync();
                        loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                        return true;
                    }
                    else
                    {
                        loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                loggingHelper.Log(ex, TraceEventType.Error);
                return false;
            }
        }
    }
}