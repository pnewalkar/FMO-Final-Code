using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.Common.SqlGeometryExtension;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Microsoft.SqlServer.Types;

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
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoadForOperationalObject(DbGeometry operationalObjectPoint, string streetName, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
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
                                                                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.PathLink).ID;

                Guid networkRoadLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                                                                    .SelectMany(x => x.ReferenceDatas)
                                                                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.RoadLink).ID;

                networkLink = DataContext.NetworkLinks.Where(m => m.StreetName_GUID == nearestNamedRoad.ID)
                   .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                   .Select(l => new NetworkLinkDTO
                   {
                       Id = l.Id,
                       LinkGeometry = l.LinkGeometry,
                       NetworkLinkType_GUID = l.NetworkLinkType_GUID
                   }).FirstOrDefault();

                if (networkLink != null)
                {
                    SqlGeometry accessLinkLine =
                        operationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry());

                    if (accessLinkLine != SqlGeometry.Null)
                    {
                        // find any road segment intersects with the planned access link.
                        var roadIntersectionCount = DataContext.NetworkLinks
                            .Count(m => m.LinkGeometry.Intersects(accessLinkLine.ToDbGeometry())
                                        && m.NetworkLinkType_GUID == networkRoadLinkType);

                        // find any path segment intersects with the planned access link.
                        var pathIntersectionCount = DataContext.NetworkLinks
                            .Count(m => m.LinkGeometry.Intersects(accessLinkLine.ToDbGeometry())
                                        && m.NetworkLinkType_GUID == networkPathLinkType);

                        if (pathIntersectionCount == 0 && roadIntersectionCount == 0)
                        {
                            networkIntersectionPoint = accessLinkLine.STEndPoint();
                        }
                    }
                }
            }

            return new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint);
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestRoadForOperationalObject(DbGeometry operationalObjectPoint, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            SqlGeometry networkIntersectionPoint = SqlGeometry.Null;

            Guid networkPathLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                .SelectMany(x => x.ReferenceDatas)
                .Single(x => x.ReferenceDataValue == ReferenceDataValues.PathLink).ID;

            Guid networkRoadLinkType = referenceDataCategoryList.Where(x => x.CategoryName == ReferenceDataCategoryNames.NetworkLinkType)
                .SelectMany(x => x.ReferenceDatas)
                .Single(x => x.ReferenceDataValue == ReferenceDataValues.RoadLink).ID;

            var networkLinkRoad = DataContext.NetworkLinks
                .Where(m => m.NetworkLinkType_GUID == networkRoadLinkType || m.NetworkLinkType_GUID == networkPathLinkType)
                .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                .Select(l => new NetworkLinkDTO
                {
                    Id = l.Id,
                    LinkGeometry = l.LinkGeometry,
                    NetworkLinkType_GUID = l.NetworkLinkType_GUID
                }).FirstOrDefault();

            if (networkLinkRoad != null)
            {
                var accessLinkLine =
                    operationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLinkRoad.LinkGeometry.ToSqlGeometry());

                if (accessLinkLine != SqlGeometry.Null)
                {
                    networkIntersectionPoint = accessLinkLine.STEndPoint();
                }
            }

            return new Tuple<NetworkLinkDTO, SqlGeometry>(networkLinkRoad, networkIntersectionPoint);
        }
    }
}