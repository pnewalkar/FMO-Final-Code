namespace RM.DataManagement.AccessLink.WebAPI.DataService.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using CommonLibrary.DataMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using DTO;
    using Entities;
    using CommonLibrary.ExceptionMiddleware;
    using CommonLibrary.HelperMiddleware;
    using Interfaces;
    using MappingConfiguration;
    using Data.AccessLink.WebAPI.Utils;
    using Data.AccessLink.WebAPI.DTO;
    using AutoMapper;

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
                    cfg.CreateMap<AccessLink,AccessLinkDataDTO >();
                    cfg.CreateMap<NetworkLink, NetworkLinkDataDTO>();
                 
                });
                Mapper.Configuration.CreateMapper();

                var accesslink = Mapper.Map<List<AccessLink> ,List<AccessLinkDataDTO>>(result);

               // var accessLink = GenericMapper.MapList<AccessLink, AccessLinkDataDTO> (result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return accesslink;
            }
        }

        /// <summary>
        /// Creates automatic access link.
        /// </summary>
        /// <param name="accessLinkDto">Access link data object.</param>
        /// <returns>Success.</returns>
        public bool CreateAccessLink(AccessLinkDTO accessLinkDto)
        {
            // to do
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAccessLink"))
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    bool accessLinkCreationSuccess = false;
                    Guid locationGuid = Guid.NewGuid();
                    Guid accessLinkGuid = Guid.NewGuid();

                    // need to save Location for Access link with shape as NetworkIntersectionPoint i.e. roadlink and access link intersection
                    Location location = new Location
                    {
                        ID = locationGuid,
                        Shape = accessLinkDto.NetworkIntersectionPoint,
                        RowCreateDateTime = DateTime.UtcNow
                    };
                    NetworkNode networkNode = new NetworkNode
                    {
                        ID = locationGuid,
                        DataProviderGUID = Guid.Empty,// AccessLinkConstants.Internal,
                        RowCreateDateTime = DateTime.UtcNow,
                        NetworkNodeType_GUID = Guid.Empty,// AccessLinkConstants.AccessLinkDataProviderGUID,
                    };
                    location.NetworkNode = networkNode;

                    NetworkLink networkLink = new NetworkLink
                    {
                        ID = accessLinkGuid,
                        DataProviderGUID = Guid.Empty,// AccessLinkConstants.Internal,
                        NetworkLinkTypeGUID = Guid.Empty,// (AccessLink)
                        StartNodeID = accessLinkDto.OperationalObject_GUID,
                        EndNodeID = locationGuid,
                        LinkLength = accessLinkDto.ActualLengthMeter,
                        LinkGeometry = accessLinkDto.AccessLinkLine,
                        RowCreateDateTime = DateTime.Now
                    };
                    AccessLink accessLink = new AccessLink
                    {
                        ID = accessLinkGuid,
                        //OperationalObjectPoint = accessLinkDto.OperationalObjectPoint,
                        //NetworkIntersectionPoint = accessLinkDto.NetworkIntersectionPoint,
                        //AccessLinkLine = accessLinkDto.AccessLinkLine,
                        //ActualLengthMeter = accessLinkDto.ActualLengthMeter,
                        WorkloadLengthMeter = accessLinkDto.WorkloadLengthMeter,
                        Approved = accessLinkDto.Approved,
                        //OperationalObject_GUID = accessLinkDto.OperationalObject_GUID,
                        ConnectedNetworkLinkID = accessLinkDto.NetworkLink_GUID,
                        AccessLinkTypeGUID = accessLinkDto.AccessLinkType_GUID,
                        //LinkStatus_GUID = accessLinkDto.LinkStatus_GUID,
                        LinkDirectionGUID = accessLinkDto.LinkDirection_GUID,
                        //OperationalObjectType_GUID = accessLinkDto.OperationalObjectType_GUID
                    };
                    AccessLinkStatus accessLinkStatus = new AccessLinkStatus
                    {
                        ID = Guid.Empty,
                        NetworkLinkID = accessLinkGuid,
                        AccessLinkStatusGUID = Guid.Empty,
                        StartDateTime = DateTime.UtcNow,
                        RowCreateDateTime = DateTime.UtcNow
                    };


                    DataContext.AccessLinks.Add(accessLink);

                    accessLinkCreationSuccess = DataContext.SaveChanges() > 0;
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkDataServiceMethodExitEventId, LoggerTraceConstants.Title);
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
        public List<AccessLinkDTO> GetAccessLinksCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            // to do
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetAccessLinksCrossingOperationalObject"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<AccessLinkDTO> accessLinkDTOs = new List<AccessLinkDTO>();
                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                List<AccessLink> crossingAccessLinks = null; // DataContext.AccessLinks.AsNoTracking().Where(al => al.AccessLinkLine != null && al.AccessLinkLine.Intersects(extent) && al.AccessLinkLine.Crosses(accessLink)).ToList();
                List<AccessLinkDTO> crossingAccessLinkDTOs = GenericMapper.MapList<AccessLink, AccessLinkDTO>(crossingAccessLinks);
                accessLinkDTOs.AddRange(crossingAccessLinkDTOs);

                List<AccessLink> overLappingAccessLinks = null; // DataContext.AccessLinks.AsNoTracking().Where(al => al.AccessLinkLine != null && al.AccessLinkLine.Intersects(extent) && al.AccessLinkLine.Overlaps(accessLink)).ToList();
                List<AccessLinkDTO> overLappingAccessLinkDTOs = GenericMapper.MapList<AccessLink, AccessLinkDTO>(overLappingAccessLinks);
                accessLinkDTOs.AddRange(overLappingAccessLinkDTOs);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return accessLinkDTOs;
            }
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>Link of Access Link Entity</returns>
        private IEnumerable<AccessLink> GetAccessLinkCoordinatesDataByBoundingBox(string boundingBoxCoordinates, Guid locationGuid)
        {
            // to do (UnitLocations is not available)
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