using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.Entities;
using AutoMapper;
using Microsoft.IdentityModel.Protocols;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;

namespace RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation
{
    /// <summary>
    /// DataService to fetch street network details
    /// </summary>
    public class StreetNetworkDataService : DataServiceBase<StreetName, NetworkDBContext>, IStreetNetworkDataService
    {
        #region Member Variables

        private const int BNGCOORDINATESYSTEM = 27700;
        private const string SearchResultCount = "SearchResultCount";

        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.StreetNetworkDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.StreetNetworkDataServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public StreetNetworkDataService(IDatabaseFactory<NetworkDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="locationID">The location unique identifier.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDataDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid locationID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchStreetNamesForAdvanceSearch"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(FetchStreetNamesForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == locationID)
                .Select(x => x.Shape).SingleOrDefault();

                var streetNames = await DataContext.StreetNames.AsNoTracking()
                    .Where(
                        l =>
                            l.Geometry.Intersects(polygon) &&
                            (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                    .Select(l => new StreetNameDataDTO
                    {
                        ID = l.ID,
                        StreetType = l.StreetType,
                        NationalRoadCode = l.NationalRoadCode,
                        DesignatedName = l.DesignatedName,
                        Descriptor = l.Descriptor
                    }).ToListAsync();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return streetNames;
            }
        }

        /// <summary>
        /// Fetch street name for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationID">The location unique identifier.</param>
        /// <returns>The result set of street name.</returns>
        public async Task<List<StreetNameDataDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid locationID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchStreetNamesForBasicSearch"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(FetchStreetNamesForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                int takeCount = Convert.ToInt32(ConfigurationSettings.AppSettings[SearchResultCount]);
                searchText = searchText ?? string.Empty;

                DbGeometry polygon =
                    DataContext.Locations.Where(x => x.ID == locationID)
                        .Select(x => x.Shape)
                        .SingleOrDefault();

                var streetNamesDto =
                    await DataContext.StreetNames.Where(
                            l =>
                                l.Geometry.Intersects(polygon) &&
                                (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                        .Take(takeCount)
                        .Select(l => new StreetNameDataDTO
                        {
                            ID = l.ID,
                            StreetType = l.StreetType,
                            NationalRoadCode = l.NationalRoadCode,
                            DesignatedName = l.DesignatedName,
                            Descriptor = l.Descriptor
                        })
                        .ToListAsync();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return streetNamesDto;
            }
        }

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationID">The location unique identifier.</param>
        /// <returns>The total count of street name</returns>
        public async Task<int> GetStreetNameCount(string searchText, Guid locationID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetStreetNameCount"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetStreetNameCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    searchText = searchText ?? string.Empty;
                    DbGeometry polygon =
                        DataContext.Locations.Where(x => x.ID == locationID)
                            .Select(x => x.Shape)
                            .SingleOrDefault();
                    var getStreetNameCount = await DataContext.StreetNames.Where(
                            l =>
                                l.Geometry.Intersects(polygon) &&
                                (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                        .CountAsync();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return getStreetNameCount;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
                catch (OverflowException overflow)
                {
                    overflow.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_OverflowException, overflow);
                }
            }
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDataDTO, List<SqlGeometry>> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNearestNamedRoad"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<SqlGeometry> listNetworkIntersectionPoints = new List<SqlGeometry>();
                SqlGeometry networkIntersectionPoint = SqlGeometry.Null;
                NetworkLinkDataDTO networkLink = null;

                // find the nearest named road for the provided operational object.
                var nearestNamedRoad = DataContext.StreetNames
                    .Where(m => m.Descriptor == streetName
                                 || m.DesignatedName == streetName
                                 || m.LocalName == streetName)
                    .OrderBy(n => operationalObjectPoint.Distance(n.Geometry))
                    .Select(l => new StreetNameDataDTO
                    {
                        ID = l.ID,
                        StreetType = l.StreetType,
                        NationalRoadCode = l.NationalRoadCode,
                        DesignatedName = l.DesignatedName,
                        Descriptor = l.Descriptor
                    }).FirstOrDefault();

                if (nearestNamedRoad != null)
                {
                    // check if the there are no intersections with any other roads and the access link
                    // intersection point
                    Guid networkPathLinkType = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkLinkType)
                                                                        .SelectMany(x => x.ReferenceDatas)
                                                                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkPathLink).ID;

                    Guid networkRoadLinkType = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkLinkType)
                                                                        .SelectMany(x => x.ReferenceDatas)
                                                                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkRoadLink).ID;

                    networkLink = DataContext.NetworkLinks.AsNoTracking().Where(m => m.StreetNameGUID == nearestNamedRoad.ID)
                       .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                       .Select(l => new NetworkLinkDataDTO
                       {
                           ID = l.ID,
                           LinkGeometry = l.LinkGeometry,
                           NetworkLinkTypeGUID = l.NetworkLinkTypeGUID,
                           TOID = l.TOID
                       }).FirstOrDefault();

                    if (networkLink != null)
                    {
                        SqlGeometry accessLinkLine =
                            operationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry());

                        if (!accessLinkLine.IsNull)
                        {
                            DbGeometry accessLinkDbGeometry = accessLinkLine.ToDbGeometry();

                            // find any road or path segment intersects with the planned access link.
                            var intersectionCountForRoadOrPath = DataContext.NetworkLinks.AsNoTracking()
                                .Count(m => m.LinkGeometry.Intersects(accessLinkDbGeometry)
                                            && (m.NetworkLinkTypeGUID == networkRoadLinkType || m.NetworkLinkTypeGUID == networkPathLinkType));

                            if (intersectionCountForRoadOrPath == 0)
                            {
                                // TODO Code to be refactored : Will be taken care by Access Link Manager 
                                // and commented lines will be removed once the implementation is done according to the new data model.

                                // var intersectionCountForDeliveryPoint = DataContext.DeliveryPoints.AsNoTracking()
                                //.Count(m => m.LocationXY.Intersects(accessLinkDbGeometry) && !m.LocationXY.SpatialEquals(operationalObjectPoint));

                                // if (intersectionCountForDeliveryPoint == 0 && !DataContext.AccessLinks.AsNoTracking().Any(a => a.AccessLinkLine.Crosses(accessLinkDbGeometry) || a.AccessLinkLine.Overlaps(accessLinkDbGeometry)))
                                // {
                                networkIntersectionPoint = accessLinkLine.STEndPoint();
                                listNetworkIntersectionPoints.Add(networkIntersectionPoint);
                                //}
                            }
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return new Tuple<NetworkLinkDataDTO, List<SqlGeometry>>(networkLink, listNetworkIntersectionPoints);
            }
        }

        /// <summary>
        /// Get the nearest segment for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDataDTO, List<SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNearestSegment"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetNearestSegment);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<SqlGeometry> listNetworkIntersectionPoints = new List<SqlGeometry>();
                SqlGeometry networkIntersectionPoint = SqlGeometry.Null;

                Guid networkPathLinkType = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkLinkType)
                                                                         .SelectMany(x => x.ReferenceDatas)
                                                                         .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkPathLink).ID;

                Guid networkRoadLinkType = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkLinkType)
                                                                    .SelectMany(x => x.ReferenceDatas)
                                                                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkRoadLink).ID;

                var accessLinkDiffRoadMaxDistance = Convert.ToInt32(referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters)
                                                                                             .SelectMany(x => x.ReferenceDatas)
                                                                                             .Single(x => x.ReferenceDataName == ReferenceDataValues.AccessLinkDiffRoadMaxDistance)
                                                                                             .ReferenceDataValue);

                var networkLinkRoads = DataContext.NetworkLinks.AsNoTracking()
                    .Where(m => (m.NetworkLinkTypeGUID == networkRoadLinkType || m.NetworkLinkTypeGUID == networkPathLinkType)
                                && m.LinkGeometry.Distance(operationalObjectPoint) <= accessLinkDiffRoadMaxDistance)
                    .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                    .AsEnumerable()
                    .Select(l => new NetworkLinkDataDTO
                    {
                        ID = l.ID,
                        LinkGeometry = l.LinkGeometry,
                        NetworkLinkTypeGUID = l.NetworkLinkTypeGUID,
                        TOID = l.TOID
                    });

                NetworkLinkDataDTO networkLinkRoad = null;


                // check for nearest segment which does not cross any existing access link
                foreach (var item in networkLinkRoads)
                {
                    var accessLinkLine =
                   operationalObjectPoint.ToSqlGeometry().ShortestLineTo(item.LinkGeometry.ToSqlGeometry());

                    if (!accessLinkLine.IsNull)
                    {
                        DbGeometry accessLinkDbGeometry = accessLinkLine.ToDbGeometry();

                        // TODO Code to be refactored : Will be taken care by Access Link Manager 
                        // and commented lines will be removed once the implementation is done according to the new data model.

                        //var intersectionCountForDeliveryPoint = DataContext.DeliveryPoints.AsNoTracking()
                        //        .Count(m => m.LocationXY.Intersects(accessLinkDbGeometry) && !m.LocationXY.SpatialEquals(operationalObjectPoint));

                        //if (intersectionCountForDeliveryPoint == 0 && !DataContext.AccessLinks.Any(a => a.AccessLinkLine.Crosses(accessLinkDbGeometry) || a.AccessLinkLine.Overlaps(accessLinkDbGeometry)))
                        //{
                        networkLinkRoad = item;
                        networkIntersectionPoint = accessLinkLine.STEndPoint();
                        listNetworkIntersectionPoints.Add(networkIntersectionPoint);
                        break;
                        // }
                    }
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return new Tuple<NetworkLinkDataDTO, List<SqlGeometry>>(networkLinkRoad, listNetworkIntersectionPoints);
            }
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>NetworkLink object.</returns>
        public NetworkLinkDataDTO GetNetworkLink(Guid networkLinkID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNetworkLink"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetNetworkLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var networkLink = DataContext.NetworkLinks.AsNoTracking().Where(x => x.ID == networkLinkID).SingleOrDefault();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<NetworkLink, NetworkLinkDataDTO>();
                });

                Mapper.Configuration.CreateMapper();
                var networkLinkDTO = Mapper.Map<NetworkLink, NetworkLinkDataDTO>(networkLink);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return networkLinkDTO;
            }
        }

        /// <summary> Get the Network Links crossing the operational Object for a given extent</summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">accesslink coordinate array</param>
        /// <returns>List<NetworkLinkDTO></returns>
        public List<NetworkLinkDataDTO> GetCrossingNetworkLink(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetCrossingNetworkLink"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetCrossingNetworkLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<NetworkLinkDataDTO> networkLinkDTOs = new List<NetworkLinkDataDTO>();
                DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                List<NetworkLink> crossingNetworkLinks = DataContext.NetworkLinks.AsNoTracking().Where(nl => nl.LinkGeometry != null && nl.LinkGeometry.Intersects(extent) && nl.LinkGeometry.Crosses(accessLink)).ToList();
                List<NetworkLinkDataDTO> crossingNetworkLinkDTOs = GenericMapper.MapList<NetworkLink, NetworkLinkDataDTO>(crossingNetworkLinks);
                networkLinkDTOs.AddRange(crossingNetworkLinkDTOs);

                List<NetworkLink> overLappingNetworkLinks = DataContext.NetworkLinks.AsNoTracking().Where(nl => nl.LinkGeometry != null && nl.LinkGeometry.Intersects(extent) && nl.LinkGeometry.Overlaps(accessLink)).ToList();
                List<NetworkLinkDataDTO> overLappingNetworkLinkDTOs = GenericMapper.MapList<NetworkLink, NetworkLinkDataDTO>(overLappingNetworkLinks);
                networkLinkDTOs.AddRange(overLappingNetworkLinkDTOs);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return networkLinkDTOs;
            }
        }

        #endregion Public Methods

    }
}