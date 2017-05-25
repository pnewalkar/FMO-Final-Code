using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fmo.Common.Constants;
using Fmo.Common.SqlGeometryExtension;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Microsoft.SqlServer.Types;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// Repository to fetch street network details
    /// </summary>
    public class StreetNetworkRepository : RepositoryBase<StreetName, FMODBContext>, IStreetNetworkRepository
    {
        public StreetNetworkRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid unitGuid)
        {
            try
            {
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid)
                    .Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                var streetNames = await DataContext.StreetNames.AsNoTracking()
                    .Where(
                        l =>
                            l.Geometry.Intersects(polygon) &&
                            (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                    .Select(l => new StreetNameDTO
                    {
                        ID = l.ID,
                        StreetType = l.StreetType,
                        NationalRoadCode = l.NationalRoadCode,
                        DesignatedName = l.DesignatedName,
                        Descriptor = l.Descriptor
                    }).ToListAsync();

                return streetNames;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch street name for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The result set of street name.
        /// </returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid unitGuid)
        {
            try
            {
                int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SearchResultCount]);
                searchText = searchText ?? string.Empty;

                DbGeometry polygon =
                    DataContext.UnitLocations.Where(x => x.ID == unitGuid)
                        .Select(x => x.UnitBoundryPolygon)
                        .SingleOrDefault();

                var streetNamesDto =
                    await DataContext.StreetNames.Where(
                            l =>
                                l.Geometry.Intersects(polygon) &&
                                (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                        .Take(takeCount)
                        .Select(l => new StreetNameDTO
                        {
                            ID = l.ID,
                            StreetType = l.StreetType,
                            NationalRoadCode = l.NationalRoadCode,
                            DesignatedName = l.DesignatedName,
                            Descriptor = l.Descriptor
                        })
                        .ToListAsync();

                return streetNamesDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The total count of street name
        /// </returns>
        public async Task<int> GetStreetNameCount(string searchText, Guid unitGuid)
        {
            try
            {
                searchText = searchText ?? string.Empty;
                DbGeometry polygon =
                    DataContext.UnitLocations.Where(x => x.ID == unitGuid)
                        .Select(x => x.UnitBoundryPolygon)
                        .SingleOrDefault();
                return
                    await DataContext.StreetNames.Where(
                            l =>
                                l.Geometry.Intersects(polygon) &&
                                (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                        .CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            SqlGeometry networkIntersectionPoint = SqlGeometry.Null;
            NetworkLinkDTO networkLink = null;

            // find the nearest named road for the provided operational object.
            var nearestNamedRoad = DataContext.StreetNames
                .Where(m => m.Descriptor == streetName
                             || m.DesignatedName == streetName
                             || m.LocalName == streetName)
                .OrderBy(n => operationalObjectPoint.Distance(n.Geometry))
                .Select(l => new StreetNameDTO
                {
                    ID = l.ID,
                    StreetType = l.StreetType,
                    NationalRoadCode = l.NationalRoadCode,
                    DesignatedName = l.DesignatedName,
                    Descriptor = l.Descriptor
                }).FirstOrDefault();

            if (nearestNamedRoad != null)
            {
                // check if the there are no intersections with any other roads and the access link intersection point
                Guid networkPathLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                                                                    .SelectMany(x => x.ReferenceDatas)
                                                                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkPathLink).ID;

                Guid networkRoadLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                                                                    .SelectMany(x => x.ReferenceDatas)
                                                                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkRoadLink).ID;

                networkLink = DataContext.NetworkLinks.AsNoTracking().Where(m => m.StreetName_GUID == nearestNamedRoad.ID)
                   .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                   .Select(l => new NetworkLinkDTO
                   {
                       Id = l.Id,
                       LinkGeometry = l.LinkGeometry,
                       NetworkLinkType_GUID = l.NetworkLinkType_GUID,
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
                                        && (m.NetworkLinkType_GUID == networkRoadLinkType || m.NetworkLinkType_GUID == networkPathLinkType));

                        if (intersectionCountForRoadOrPath == 0)
                        {
                            var intersectionCountForDeliveryPoint = DataContext.DeliveryPoints.AsNoTracking()
                           .Count(m => m.LocationXY.Intersects(accessLinkDbGeometry) && !m.LocationXY.SpatialEquals(operationalObjectPoint));

                            if (intersectionCountForDeliveryPoint == 0 && !DataContext.AccessLinks.AsNoTracking().Any(a => a.AccessLinkLine.Crosses(accessLinkDbGeometry) || a.AccessLinkLine.Overlaps(accessLinkDbGeometry)))
                            {
                                networkIntersectionPoint = accessLinkLine.STEndPoint();
                            }
                        }
                    }
                }
            }

            return new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint);
        }

        /// <summary>
        /// Get the nearest segment for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestSegment(DbGeometry operationalObjectPoint, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            SqlGeometry networkIntersectionPoint = SqlGeometry.Null;

            Guid networkPathLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                                                                     .SelectMany(x => x.ReferenceDatas)
                                                                     .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkPathLink).ID;

            Guid networkRoadLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                                                                .SelectMany(x => x.ReferenceDatas)
                                                                .Single(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkRoadLink).ID;

            var accessLinkDiffRoadMaxDistance = Convert.ToInt32(referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.AccessLinkParameters)
                                                                                         .SelectMany(x => x.ReferenceDatas)
                                                                                         .Single(x => x.ReferenceDataName == ReferenceDataValues.AccessLinkDiffRoadMaxDistance)
                                                                                         .ReferenceDataValue);

            var networkLinkRoads = DataContext.NetworkLinks.AsNoTracking()
                .Where(m => (m.NetworkLinkType_GUID == networkRoadLinkType || m.NetworkLinkType_GUID == networkPathLinkType)
                            && m.LinkGeometry.Distance(operationalObjectPoint) <= accessLinkDiffRoadMaxDistance)
                .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                .AsEnumerable()
                .Select(l => new NetworkLinkDTO
                {
                    Id = l.Id,
                    LinkGeometry = l.LinkGeometry,
                    NetworkLinkType_GUID = l.NetworkLinkType_GUID,
                    TOID = l.TOID
                });

            NetworkLinkDTO networkLinkRoad = null;

            // check for nearest segment which does not cross any existing access link
            foreach (var item in networkLinkRoads)
            {
                var accessLinkLine =
               operationalObjectPoint.ToSqlGeometry().ShortestLineTo(item.LinkGeometry.ToSqlGeometry());

                if (!accessLinkLine.IsNull)
                {
                    DbGeometry accessLinkDbGeometry = accessLinkLine.ToDbGeometry();

                    var intersectionCountForDeliveryPoint = DataContext.DeliveryPoints.AsNoTracking()
                            .Count(m => m.LocationXY.Intersects(accessLinkDbGeometry) && !m.LocationXY.SpatialEquals(operationalObjectPoint));

                    if (intersectionCountForDeliveryPoint == 0 && !DataContext.AccessLinks.Any(a => a.AccessLinkLine.Crosses(accessLinkDbGeometry) || a.AccessLinkLine.Overlaps(accessLinkDbGeometry)))
                    {
                        networkLinkRoad = item;
                        networkIntersectionPoint = accessLinkLine.STEndPoint();

                        break;
                    }
                }
            }

            return new Tuple<NetworkLinkDTO, SqlGeometry>(networkLinkRoad, networkIntersectionPoint);
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>NetworkLink object.</returns>
        public NetworkLinkDTO GetNetworkLink(Guid networkLinkID)
        {
            var networkLink = DataContext.NetworkLinks.AsNoTracking().Where(x => x.Id == networkLinkID).SingleOrDefault();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<NetworkLink, NetworkLinkDTO>();
            });

            Mapper.Configuration.CreateMapper();
            var networkLinkDTO = Mapper.Map<NetworkLink, NetworkLinkDTO>(networkLink);

            return networkLinkDTO;
        }

        /// <summary>
        /// Get the Network Links crossing access link
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<NetworkLinkDTO></returns>
        public List<NetworkLinkDTO> GetCrossingNetworkLink(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            List<NetworkLinkDTO> networkLinkDTOs = new List<NetworkLinkDTO>();
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);
            List<NetworkLink> crossingNetworkLinks = DataContext.NetworkLinks.Where(nl => nl.LinkGeometry != null && nl.LinkGeometry.Intersects(extent) && nl.LinkGeometry.Crosses(accessLink)).ToList();
            List<NetworkLinkDTO> crossingNetworkLinkDTOs = GenericMapper.MapList<NetworkLink, NetworkLinkDTO>(crossingNetworkLinks);
            networkLinkDTOs.AddRange(crossingNetworkLinkDTOs);
            List<NetworkLink> overLappingNetworkLinks = DataContext.NetworkLinks.Where(nl => nl.LinkGeometry != null && nl.LinkGeometry.Intersects(extent) && nl.LinkGeometry.Overlaps(accessLink)).ToList();
            List<NetworkLinkDTO> overLappingNetworkLinkDTOs = GenericMapper.MapList<NetworkLink, NetworkLinkDTO>(overLappingNetworkLinks);
            networkLinkDTOs.AddRange(overLappingNetworkLinkDTOs);
            return networkLinkDTOs;
        }
    }
}