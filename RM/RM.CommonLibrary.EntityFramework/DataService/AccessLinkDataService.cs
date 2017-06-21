namespace RM.CommonLibrary.EntityFramework.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using DataMiddleware;
    using DTO;
    using Entities;
    using ExceptionMiddleware;
    using HelperMiddleware;
    using Interfaces;
    using LoggingMiddleware;
    using MappingConfiguration;

    /// <summary>
    /// This class contains methods of Access Link DataService for fetching Access Link data.
    /// </summary>
    public class AccessLinkDataService : DataServiceBase<AccessLink, RMDBContext>, IAccessLinkDataService
    {
        private const int BNGCOORDINATESYSTEM = 27700;

        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public AccessLinkDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
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
        public List<AccessLinkDTO> GetAccessLinks(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<AccessLink> result = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();
            var accessLink = GenericMapper.MapList<AccessLink, AccessLinkDTO>(result);
            return accessLink;
        }

        /// <summary>
        /// Creates automatic access link.
        /// </summary>
        /// <param name="accessLinkDto">Access link data object.</param>
        /// <returns>Success.</returns>
        public bool CreateAccessLink(AccessLinkDTO accessLinkDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAccessLink"))
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    bool accessLinkCreationSuccess = false;

                    AccessLink accessLink = new AccessLink
                    {
                        ID = Guid.NewGuid(),
                        OperationalObjectPoint = accessLinkDto.OperationalObjectPoint,
                        NetworkIntersectionPoint = accessLinkDto.NetworkIntersectionPoint,
                        AccessLinkLine = accessLinkDto.AccessLinkLine,
                        ActualLengthMeter = accessLinkDto.ActualLengthMeter,
                        WorkloadLengthMeter = accessLinkDto.WorkloadLengthMeter,
                        Approved = accessLinkDto.Approved,
                        OperationalObject_GUID = accessLinkDto.OperationalObject_GUID,
                        NetworkLink_GUID = accessLinkDto.NetworkLink_GUID,
                        AccessLinkType_GUID = accessLinkDto.AccessLinkType_GUID,
                        LinkStatus_GUID = accessLinkDto.LinkStatus_GUID,
                        LinkDirection_GUID = accessLinkDto.LinkDirection_GUID,
                        OperationalObjectType_GUID = accessLinkDto.OperationalObjectType_GUID
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
            List<AccessLinkDTO> accessLinkDTOs = new List<AccessLinkDTO>();
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

            List<AccessLink> crossingAccessLinks = DataContext.AccessLinks.AsNoTracking().Where(al => al.AccessLinkLine != null && al.AccessLinkLine.Intersects(extent) && al.AccessLinkLine.Crosses(accessLink)).ToList();
            List<AccessLinkDTO> crossingAccessLinkDTOs = GenericMapper.MapList<AccessLink, AccessLinkDTO>(crossingAccessLinks);
            accessLinkDTOs.AddRange(crossingAccessLinkDTOs);

            List<AccessLink> overLappingAccessLinks = DataContext.AccessLinks.AsNoTracking().Where(al => al.AccessLinkLine != null && al.AccessLinkLine.Intersects(extent) && al.AccessLinkLine.Overlaps(accessLink)).ToList();
            List<AccessLinkDTO> overLappingAccessLinkDTOs = GenericMapper.MapList<AccessLink, AccessLinkDTO>(overLappingAccessLinks);
            accessLinkDTOs.AddRange(overLappingAccessLinkDTOs);

            return accessLinkDTOs;
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>Link of Access Link Entity</returns>
        private IEnumerable<AccessLink> GetAccessLinkCoordinatesDataByBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);
                return DataContext.AccessLinks.AsNoTracking().Where(x => x.AccessLinkLine.Intersects(extent) && x.AccessLinkLine.Intersects(polygon)).ToList();
            }

            return null;
        }
    }
}