using RM.CommonLibrary.EntityFramework.Entities;
using RM.DataManagement.AccessLink.WebAPI.DataDTOs;
using RM.DataManagement.AccessLink.WebAPI.DTOs;
using System.Collections.Generic;

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
    using Entities;
    using CommonLibrary.ExceptionMiddleware;
    using CommonLibrary.HelperMiddleware;
    using Interfaces;
    using MappingConfiguration;
    using AutoMapper;
    using DTOs;
    using DataDTOs;
  

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
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return accesslink;
            }
        }

        /// <summary>
        /// Creates automatic access link.
        /// </summary>
        /// <param name="accessLinkDto">Access link data object.</param>
        /// <returns>Success.</returns>
        public bool CreateAccessLink(NetworkLinkDataDTO networkLinkDataDTO)
        {
            // TODO
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAccessLink"))
                {
                    string methodName = typeof(AccessLinkDataService) + "." + nameof(CreateAccessLink);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    bool accessLinkCreationSuccess = false;
                    //Guid locationGuid = Guid.NewGuid();
                    //Guid accessLinkGuid = Guid.NewGuid();
                    //Guid NetworkNodeTypeGuid = Guid.NewGuid();
                    AccessLink accessLink = new Entities.AccessLink();
                    // need to save Location for Access link with shape as NetworkIntersectionPoint i.e. roadlink and access link intersection
                    NetworkNode networkNode = new NetworkNode();
                    NetworkLink networkLink = new Entities.NetworkLink();



                    networkLink.NetworkNode.Location.ID = networkLinkDataDTO.NetworkNodeDataDTO.LocationDatatDTO.ID;
                    networkLink.NetworkNode.Location.Shape = networkLinkDataDTO.NetworkNodeDataDTO.LocationDatatDTO.Shape;
                    networkLink.NetworkNode.Location.RowCreateDateTime = networkLinkDataDTO.NetworkNodeDataDTO.LocationDatatDTO.RowCreateDateTime;


                    networkLink.NetworkNode1.Location.ID = networkLinkDataDTO.NetworkNodeDataDTO1.LocationDatatDTO.ID;
                    networkLink.NetworkNode1.Location.Shape = networkLinkDataDTO.NetworkNodeDataDTO1.LocationDatatDTO.Shape;
                    networkLink.NetworkNode1.Location.RowCreateDateTime = networkLinkDataDTO.NetworkNodeDataDTO1.LocationDatatDTO.RowCreateDateTime;




                    networkLink.ID = networkLinkDataDTO.ID;// accessLinkGuid,
                    networkLink.DataProviderGUID = networkLinkDataDTO.DataProviderGUID;//Guid.Empty,// AccessLinkConstants.Internal,
                    networkLink.NetworkLinkTypeGUID = networkLinkDataDTO.NetworkLinkTypeGUID;//Guid.Empty,// (AccessLink)
                    networkLink.StartNodeID = networkLinkDataDTO.StartNodeID;//accessLinkDto.OperationalObject_GUID,
                    networkLink.EndNodeID = networkLinkDataDTO.EndNodeID;//locationGuid,
                    networkLink.LinkLength = networkLinkDataDTO.LinkLength;//accessLinkDto.ActualLengthMeter,
                    networkLink.LinkGeometry = networkLinkDataDTO.LinkGeometry;//accesaccessLinkDto.AccessLinkLine,
                       networkLink.RowCreateDateTime = networkLinkDataDTO.RowCreateDateTime;


                    AccessLinkDataDTO accessLinkDataDTO = networkLinkDataDTO.AccessLinkDataDTOs;

                  
                        accessLink.ID = accessLinkDataDTO.ID;
                    //OperationalObjectPoint = accessLinkDto.OperationalObjectPoint,
                    //NetworkIntersectionPoint = accessLinkDto.NetworkIntersectionPoint,
                    //AccessLinkLine = accessLinkDto.AccessLinkLine,
                    //ActualLengthMeter = accessLinkDto.ActualLengthMeter,
                    accessLink.WorkloadLengthMeter = accessLinkDataDTO.WorkloadLengthMeter;
                    accessLink.Approved = accessLinkDataDTO.Approved;
                    //OperationalObject_GUID = accessLinkDto.OperationalObject_GUID,
                    accessLink.ConnectedNetworkLinkID = new Guid()  ;
                    accessLink.AccessLinkTypeGUID = accessLinkDataDTO.AccessLinkTypeGUID;
                    //LinkStatus_GUID = accessLinkDto.LinkStatus_GUID,
                    accessLink.LinkDirectionGUID = accessLinkDataDTO.LinkDirectionGUID;
                    //OperationalObjectType_GUID = accessLinkDto.OperationalObjectType_GUID
                    
                

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

                    networkLink.AccessLink = accessLink;
                    DataContext.NetworkLinks.Add(networkLink);

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