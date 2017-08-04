using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.SqlServer.Types;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.Entities;

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
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.StreetNetworkDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.StreetNetworkDataServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public StreetNetworkDataService(IDatabaseFactory<NetworkDBContext> databaseFactory, ILoggingHelper loggingHelper, IConfigurationHelper configurationHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="locationID">The location unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDataDTO>> GetStreetNamesForAdvanceSearch(string searchText, Guid locationID, string currentUserUnitType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetStreetNamesForAdvanceSearch"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetStreetNamesForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                DbGeometry polygon = null;
                List<StreetNameDataDTO> streetNamesDto = new List<StreetNameDataDTO>();

                if (!currentUserUnitType.Equals(UserUnit.National.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    polygon = DataContext.Locations.Where(x => x.ID == locationID)
                            .Select(x => x.Shape)
                            .SingleOrDefault();
                }

                if (polygon != null)
                {
                    streetNamesDto = await DataContext.StreetNames.AsNoTracking()
                    .Where(
                        l =>
                            l.Geometry.Intersects(polygon) &&
                            (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                    .Select(l => new StreetNameDataDTO
                    {
                        ID = l.ID,
                        NationalRoadCode = l.NationalRoadCode,
                        DesignatedName = l.DesignatedName
                    }).ToListAsync();
                }
                else
                {
                    streetNamesDto = await DataContext.StreetNames.AsNoTracking()
                    .Where(
                        l => l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText))
                    .Select(l => new StreetNameDataDTO
                    {
                        ID = l.ID,
                        NationalRoadCode = l.NationalRoadCode,
                        DesignatedName = l.DesignatedName
                    }).ToListAsync();
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return streetNamesDto;
            }
        }

        /// <summary>
        /// Fetch street name for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationID">The location unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>The result set of street name.</returns>
        public async Task<List<StreetNameDataDTO>> GetStreetNamesForBasicSearch(string searchText, Guid locationID, string currentUserUnitType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetStreetNamesForBasicSearch"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetStreetNamesForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                int takeCount = Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(SearchResultCount));
                searchText = searchText ?? string.Empty;
                DbGeometry polygon = null;
                List<StreetNameDataDTO> streetNamesDto = new List<StreetNameDataDTO>();

                if (!currentUserUnitType.Equals(UserUnit.National.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    polygon = DataContext.Locations.Where(x => x.ID == locationID)
                            .Select(x => x.Shape)
                            .SingleOrDefault();
                }

                if (polygon != null)
                {
                    streetNamesDto =
                    await DataContext.StreetNames.Where(
                            l =>
                                l.Geometry.Intersects(polygon) &&
                                (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                        .Take(takeCount)
                        .Select(l => new StreetNameDataDTO
                        {
                            ID = l.ID,
                            NationalRoadCode = l.NationalRoadCode,
                            DesignatedName = l.DesignatedName
                        }).ToListAsync();
                }
                else
                {
                    streetNamesDto =
                        await DataContext.StreetNames.Where(
                                l => l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText))
                            .Take(takeCount)
                            .Select(l => new StreetNameDataDTO
                            {
                                ID = l.ID,
                                NationalRoadCode = l.NationalRoadCode,
                                DesignatedName = l.DesignatedName
                            }).ToListAsync();
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return streetNamesDto;
            }
        }

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationID">The location unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>The total count of street name</returns>
        public async Task<int> GetStreetNameCount(string searchText, Guid locationID, string currentUserUnitType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetStreetNameCount"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetStreetNameCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    searchText = searchText ?? string.Empty;
                    DbGeometry polygon = null;
                    int streetNameCount = default(int);
                    List<StreetNameDataDTO> streetNamesDto = new List<StreetNameDataDTO>();

                    if (!currentUserUnitType.Equals(UserUnit.National.GetDescription(), StringComparison.OrdinalIgnoreCase))
                    {
                        polygon = DataContext.Locations.Where(x => x.ID == locationID)
                                .Select(x => x.Shape)
                                .SingleOrDefault();
                    }

                    if (polygon != null)
                    {
                        streetNameCount = await DataContext.StreetNames.Where(
                            l =>
                                l.Geometry.Intersects(polygon) &&
                                (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                        .CountAsync();
                    }
                    else
                    {
                        streetNameCount = await DataContext.StreetNames.Where(
                            l => l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText))
                        .CountAsync();
                    }

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return streetNameCount;
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
        public Tuple<NetworkLinkDataDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNearestNamedRoad"))
            {
                string methodName = typeof(StreetNetworkDataService) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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
                                networkIntersectionPoint = accessLinkLine.STEndPoint();
                            }
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return new Tuple<NetworkLinkDataDTO, SqlGeometry>(networkLink, networkIntersectionPoint);
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

                        networkLinkRoad = item;
                        networkIntersectionPoint = accessLinkLine.STEndPoint();
                        listNetworkIntersectionPoints.Add(networkIntersectionPoint);
                        break;
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