using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// DataService to interact with postal address entity
    /// </summary>
    public class PostCodeDataService : DataServiceBase<Postcode, RMDBContext>, IPostCodeDataService
    {
        private const string SearchResultCount = "SearchResultCount";

        public PostCodeDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch postcode for basic search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The result set of post code
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid unitGuid)
        {
            int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[SearchResultCount]);
            searchText = searchText ?? string.Empty;
            var postCodeDetailsDto = await (from p in DataContext.Postcodes.AsNoTracking()
                                            join s in DataContext.PostcodeSectors.AsNoTracking() on p.SectorGUID equals s.ID
                                            join u in DataContext.UnitPostcodeSectors.AsNoTracking() on s.ID equals u.PostcodeSector_GUID
                                            where p.PostcodeUnit.StartsWith(searchText)
                                             && u.Unit_GUID == unitGuid
                                            select new PostCodeDTO
                                            {
                                                PostcodeUnit = p.PostcodeUnit,
                                                InwardCode = p.InwardCode,
                                                OutwardCode = p.OutwardCode,
                                                Sector = p.Sector
                                            }).Take(takeCount).ToListAsync();

            return postCodeDetailsDto;
        }

        /// <summary>
        /// Get the count of post code
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The total count of post code
        /// </returns>
        public async Task<int> GetPostCodeUnitCount(string searchText, Guid unitGuid)
        {
            searchText = searchText ?? string.Empty;
            var postCodeDetailsDto = await (from p in DataContext.Postcodes.AsNoTracking()
                                            join s in DataContext.PostcodeSectors.AsNoTracking() on p.SectorGUID equals s.ID
                                            join u in DataContext.UnitPostcodeSectors.AsNoTracking() on s.ID equals u.PostcodeSector_GUID
                                            where p.PostcodeUnit.StartsWith(searchText)
                                             && u.Unit_GUID == unitGuid
                                            select p).CountAsync();
            return postCodeDetailsDto;
        }

        /// <summary>
        /// Fetches the post code unit for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<PostCodeDTO></returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid unitGuid)
        {
            searchText = searchText ?? string.Empty;
            var postCodeDetailsDto = await (from p in DataContext.Postcodes.AsNoTracking()
                                            join s in DataContext.PostcodeSectors.AsNoTracking() on p.SectorGUID equals s.ID
                                            join u in DataContext.UnitPostcodeSectors.AsNoTracking() on s.ID equals u.PostcodeSector_GUID
                                            where p.PostcodeUnit.StartsWith(searchText)
                                             && u.Unit_GUID == unitGuid
                                            select new PostCodeDTO
                                            {
                                                PostcodeUnit = p.PostcodeUnit,
                                                InwardCode = p.InwardCode,
                                                OutwardCode = p.OutwardCode,
                                                Sector = p.Sector
                                            }).ToListAsync();

            return postCodeDetailsDto;
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            var postCodeDetail = await DataContext.Postcodes.Where(l => l.PostcodeUnit.Trim().Equals(postCode, StringComparison.OrdinalIgnoreCase)).SingleOrDefaultAsync();
            if (postCodeDetail != null)
            {
                return postCodeDetail.ID;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}