using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// Repository to interact with postal address entity
    /// </summary>
    public class PostCodeRepository : RepositoryBase<Postcode, FMODBContext>, IPostCodeRepository
    {
        public PostCodeRepository(IDatabaseFactory<FMODBContext> databaseFactory)
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
            try
            {
                int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SearchResultCount]);
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
            catch (Exception)
            {
                throw;
            }
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
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches the post code unit for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<PostCodeDTO></returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid unitGuid)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public Guid GetPostCodeID(string postCode)
        {
            var postCodeDetail = DataContext.Postcodes.Where(l => l.PostcodeUnit.Trim().Equals(postCode, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
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