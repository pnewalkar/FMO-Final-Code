﻿namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class PostCodeRepository : RepositoryBase<Postcode, FMODBContext>, IPostCodeRepository
    {
        public PostCodeRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<List<PostCodeDTO>> FetchPostCodeUnitforBasicSearch(string searchText)
        {
            try
            {
                int takeCount = 5;
                var postCodeDetails = await DataContext.Postcodes.Where(l => l.PostcodeUnit.StartsWith(searchText)).Take(takeCount).ToListAsync();
                return GenericMapper.MapList<Postcode, PostCodeDTO>(postCodeDetails.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetPostCodeUnitCount(string searchText)
        {
            try
            {
                return await DataContext.Postcodes.Where(l => l.PostcodeUnit.StartsWith(searchText)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PostCodeDTO>> FetchPostCodeUnit(string searchText)
        {
            try
            {
                var postCodeDetails = DataContext.Postcodes.Where(l => l.PostcodeUnit.StartsWith(searchText)).ToList();

                var result = await DataContext.Postcodes.ToListAsync();
                return GenericMapper.MapList<Postcode, PostCodeDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
