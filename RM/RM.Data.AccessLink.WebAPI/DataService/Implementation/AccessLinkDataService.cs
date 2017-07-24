namespace RM.DataManagement.AccessLink.WebAPI.DataService.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using AutoMapper;
    using CommonLibrary.DataMiddleware;
    using CommonLibrary.ExceptionMiddleware;
    using CommonLibrary.HelperMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using Data.AccessLink.WebAPI.DataDTOs;
    using Entities;
    using Interfaces;
    using MappingConfiguration;

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

                var result = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<AccessLink, AccessLinkDataDTO>();
                    cfg.CreateMap<NetworkLink, NetworkLinkDataDTO>();
                });

                Mapper.Configuration.CreateMapper();

                var resultValue = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<AccessLink, AccessLinkDataDTO>();
                    cfg.CreateMap<NetworkLink, NetworkLinkDataDTO>();
                });
                Mapper.Configuration.CreateMapper();

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
        public bool CreateAutomaticAccessLink(NetworkLinkDataDTO networkLinkDataDTO)
        {
            // TODO
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAutomaticAccessLink"))
                {
                    string methodName = typeof(AccessLinkDataService) + "." + nameof(CreateAutomaticAccessLink);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    bool accessLinkCreationSuccess = false;
                    AccessLink accessLink = new Entities.AccessLink();

                    // need to save Location for Access link with shape as NetworkIntersectionPoint i.e. roadlink and access link intersection
                    NetworkNode networkNode = new NetworkNode();
                    NetworkLink networkLink = new Entities.NetworkLink();
                    NetworkLink networkLink1 = DataContext.NetworkLinks.Where(n => n.ID == networkLinkDataDTO.ID).SingleOrDefault();
                    networkLink.NetworkNode = new Entities.NetworkNode();
                    networkLink.NetworkNode.Location = new Entities.Location();
                    networkLink.NetworkNode.Location = new Location();
                    networkLink.NetworkNode.Location.ID = networkLinkDataDTO.NetworkNodeDataDTO.LocationDatatDTO.ID;
                    networkLink.NetworkNode.Location.Shape = networkLinkDataDTO.NetworkNodeDataDTO.LocationDatatDTO.Shape;
                    networkLink.NetworkNode.Location.RowCreateDateTime = networkLinkDataDTO.NetworkNodeDataDTO.LocationDatatDTO.RowCreateDateTime;

                    networkLink.ID = networkLinkDataDTO.ID;
                    networkLink.DataProviderGUID = networkLinkDataDTO.DataProviderGUID;
                    networkLink.NetworkLinkTypeGUID = networkLinkDataDTO.NetworkLinkTypeGUID;
                    networkLink.StartNodeID = networkLinkDataDTO.StartNodeID;
                    networkLink.EndNodeID = networkLinkDataDTO.EndNodeID;
                    networkLink.LinkLength = networkLinkDataDTO.LinkLength;
                    networkLink.LinkGeometry = networkLinkDataDTO.LinkGeometry;
                    networkLink.RowCreateDateTime = networkLinkDataDTO.RowCreateDateTime;
                    AccessLinkDataDTO accessLinkDataDTO = networkLinkDataDTO.AccessLinkDataDTOs;

                    accessLink.ID = accessLinkDataDTO.ID;
                    accessLink.WorkloadLengthMeter = accessLinkDataDTO.WorkloadLengthMeter;
                    accessLink.Approved = accessLinkDataDTO.Approved;
                    accessLink.ConnectedNetworkLinkID = new Guid();
                    accessLink.AccessLinkTypeGUID = accessLinkDataDTO.AccessLinkTypeGUID;
                    accessLink.LinkDirectionGUID = accessLinkDataDTO.LinkDirectionGUID;
                    accessLink.RowCreateDateTime = DateTime.UtcNow;
                    networkLink.AccessLink = accessLink;

                    AccessLinkStatusDataDTO accessLinkStatusDataDTO = accessLinkDataDTO.AccessLinkStatusDataDTO;
                    AccessLinkStatus accessLinkStatus = new AccessLinkStatus
                    {
                        ID = accessLinkStatusDataDTO.ID,
                        NetworkLinkID = accessLinkStatusDataDTO.NetworkLinkID,
                        AccessLinkStatusGUID = accessLinkStatusDataDTO.AccessLinkStatusGUID,
                        StartDateTime = accessLinkStatusDataDTO.StartDateTime,
                        RowCreateDateTime = accessLinkStatusDataDTO.RowCreateDateTime
                    };
                    accessLink.AccessLinkStatus.Add(accessLinkStatus);
                    accessLink.NetworkLink = networkLink1;
                    accessLink.NetworkLink1 = networkLink1;
                    DataContext.AccessLinks.Add(accessLink);
                    accessLinkCreationSuccess = DataContext.SaveChanges() > 0;
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return accessLinkCreationSuccess;
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("automatic access link")));
            }
        }

        /// <summary>
        /// Method to create manual access link in db
        /// </summary>
        /// <param name="networkLinkDataDTO"></param>
        /// <returns></returns>
        public bool CreateManualAccessLink(NetworkLinkDataDTO networkLinkDataDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateManualAccessLink"))
                {
                    string methodName = typeof(AccessLinkDataService) + "." + nameof(CreateManualAccessLink);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    bool accessLinkCreationSuccess = false;
                    AccessLink accessLink = new Entities.AccessLink();

                    NetworkNode networkNode = new NetworkNode();
                    NetworkLink networkLink = new Entities.NetworkLink();

                    NetworkLink networkLink1 = DataContext.NetworkLinks.Where(n => n.ID == networkLinkDataDTO.ID).SingleOrDefault();
                    AccessLinkDataDTO accessLinkDataDTO = networkLinkDataDTO.AccessLinkDataDTOs;

                    accessLink.ID = accessLinkDataDTO.ID;
                    accessLink.WorkloadLengthMeter = accessLinkDataDTO.WorkloadLengthMeter;
                    accessLink.Approved = accessLinkDataDTO.Approved;
                    accessLink.ConnectedNetworkLinkID = new Guid();
                    accessLink.AccessLinkTypeGUID = accessLinkDataDTO.AccessLinkTypeGUID;
                    accessLink.LinkDirectionGUID = accessLinkDataDTO.LinkDirectionGUID;
                    accessLink.RowCreateDateTime = DateTime.UtcNow;

                    // networkLink.AccessLink = accessLink;
                    AccessLinkStatusDataDTO accessLinkStatusDataDTO = accessLinkDataDTO.AccessLinkStatusDataDTO;
                    AccessLinkStatus accessLinkStatus = new AccessLinkStatus
                    {
                        ID = accessLinkStatusDataDTO.ID,
                        NetworkLinkID = accessLinkStatusDataDTO.NetworkLinkID,
                        AccessLinkStatusGUID = accessLinkStatusDataDTO.AccessLinkStatusGUID,
                        StartDateTime = accessLinkStatusDataDTO.StartDateTime,
                        RowCreateDateTime = accessLinkStatusDataDTO.RowCreateDateTime
                    };
                    accessLink.AccessLinkStatus.Add(accessLinkStatus);

                    accessLink.NetworkLink = networkLink1;
                    accessLink.NetworkLink1 = networkLink1;
                    DataContext.AccessLinks.Add(accessLink);

                    accessLinkCreationSuccess = DataContext.SaveChanges() > 0;

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return accessLinkCreationSuccess;
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
        public List<AccessLinkDataDTO> GetAccessLinksCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetAccessLinksCrossingOperationalObject"))
            {
                string methodName = typeof(AccessLinkDataService) + "." + nameof(GetAccessLinksCrossingOperationalObject);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<AccessLinkDataDTO> accessLinkDataDTOs = new List<AccessLinkDataDTO>();
                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                List<AccessLink> crossingAccessLinks = null; // DataContext.AccessLinks.AsNoTracking().Where(al => al.AccessLinkLine != null && al.AccessLinkLine.Intersects(extent) && al.AccessLinkLine.Crosses(accessLink)).ToList();
                List<AccessLinkDataDTO> crossingAccessLinkDTOs = GenericMapper.MapList<AccessLink, AccessLinkDataDTO>(crossingAccessLinks);
                accessLinkDataDTOs.AddRange(crossingAccessLinkDTOs);

                List<AccessLink> overLappingAccessLinks = null; // DataContext.AccessLinks.AsNoTracking().Where(al => al.AccessLinkLine != null && al.AccessLinkLine.Intersects(extent) && al.AccessLinkLine.Overlaps(accessLink)).ToList();
                List<AccessLinkDataDTO> overLappingAccessLinkDTOs = GenericMapper.MapList<AccessLink, AccessLinkDataDTO>(overLappingAccessLinks);
                accessLinkDataDTOs.AddRange(overLappingAccessLinkDTOs);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return accessLinkDataDTOs;
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
        public int GetAccessLinkCountForCrossesorOverLaps(DbGeometry operationalObjectPoint, DbGeometry accessLink)
        {
            var accesslinkCount = DataContext.NetworkLinks.AsNoTracking().Where(al => al.LinkGeometry != null && al.LinkGeometry.Intersects(accessLink) && al.LinkGeometry.Crosses(accessLink)).ToList();

            return accesslinkCount.Count;
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
    }
}