using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class PostalAddressRepository : RepositoryBase<PostalAddress, FMODBContext>, IPostalAddressRepository
    {
        public PostalAddressRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<List<PostalAddressDTO>> FetchPostalAddressforBasicSearch(string searchText)
        {
            try
            {
                int takeCount = 5;
                var postalAddressDto = await DataContext.PostalAddresses.Where(l => l.OrganisationName.Contains(searchText)
                                    || l.BuildingName.Contains(searchText)
                                    || l.SubBuildingName.Contains(searchText)
                                    || SqlFunctions.StringConvert((double)l.BuildingNumber).StartsWith(searchText)
                                    || l.Thoroughfare.Contains(searchText)
                                    || l.DependentLocality.Contains(searchText))
                    .Take(takeCount)
                    .Select(l => new PostalAddressDTO
                    {
                        Address_Id = l.Address_Id,
                        AddressType_Id = l.AddressType_Id,
                        OrganisationName = l.OrganisationName,
                        BuildingName = l.BuildingName,
                        SubBuildingName = l.SubBuildingName,
                        BuildingNumber = l.BuildingNumber,
                        Thoroughfare = l.Thoroughfare,
                        DependentLocality = l.DependentLocality
                    })
                    .ToListAsync();

                return postalAddressDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetPostalAddressCount(string searchText)
        {
            try
            {
                return await DataContext.PostalAddresses.Where(l => l.OrganisationName.Contains(searchText)
                                    || l.BuildingName.Contains(searchText)
                                    || l.SubBuildingName.Contains(searchText)
                                    || SqlFunctions.StringConvert((double)l.BuildingNumber).StartsWith(searchText)
                                    || l.Thoroughfare.Contains(searchText)
                                    || l.DependentLocality.Contains(searchText))
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
