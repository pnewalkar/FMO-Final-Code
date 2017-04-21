namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;

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
                    .Where(l => l.Geometry.Intersects(polygon) && (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                    .Select(l => new StreetNameDTO
                    {
                        StreetName_Id = l.StreetName_Id,
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

                DbGeometry polygon = DataContext.UnitLocations.Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                var streetNamesDto = await DataContext.StreetNames.Where(l => l.Geometry.Intersects(polygon) && (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                    .Take(takeCount)
                    .Select(l => new StreetNameDTO
                    {
                        StreetName_Id = l.StreetName_Id,
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
                DbGeometry polygon = DataContext.UnitLocations.Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();
                return await DataContext.StreetNames.Where(l => l.Geometry.Intersects(polygon) && (l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText)))
                    .CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}