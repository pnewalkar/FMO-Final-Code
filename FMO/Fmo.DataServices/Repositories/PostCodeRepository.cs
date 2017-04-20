﻿namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

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
        /// <returns>The result set of post code</returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText)
        {
            try
            {
                int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SearchResultCount]);
                searchText = searchText ?? string.Empty;
                var postCodeDetailsDto = await DataContext.Postcodes.Where(l => l.PostcodeUnit.StartsWith(searchText))
                    .Take(takeCount)
                    .Select(l => new PostCodeDTO
                    {
                        PostcodeUnit = l.PostcodeUnit,
                        InwardCode = l.InwardCode,
                        OutwardCode = l.OutwardCode,
                        Sector = l.Sector
                    }).ToListAsync();

                return postCodeDetailsDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the count of post code
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <returns>The total count of post code</returns>
        public async Task<int> GetPostCodeUnitCount(string searchText)
        {
            try
            {
                searchText = searchText ?? string.Empty;
                return await DataContext.Postcodes.Where(l => l.PostcodeUnit.StartsWith(searchText)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetch Postcode Unit for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>PostCodeDetails</returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText)
        {
            try
            {
                var postCodeDetails = await DataContext.Postcodes.Where(l => l.PostcodeUnit.StartsWith(searchText)).ToListAsync();

                return GenericMapper.MapList<Postcode, PostCodeDTO>(postCodeDetails);
            }
            catch (Exception ex)
            {
                throw ex;
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