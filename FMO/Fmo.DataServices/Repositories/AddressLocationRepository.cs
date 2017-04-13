﻿namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Linq;
    using DTO;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using MappingConfiguration;
    using System.Threading.Tasks;

    public class AddressLocationRepository : RepositoryBase<AddressLocation, FMODBContext>, IAddressLocationRepository
    {
        public AddressLocationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool AddressLocationExists(int uDPRN)
        {
            try
            {
                if (DataContext.AddressLocations.Where(n => n.UDPRN == uDPRN).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddressLocationDTO GetAddressLocationByUDPRN(int uDPRN)
        {
            try
            {
                var objAddressLocation = DataContext.AddressLocations.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                var addressLocationDTO = new AddressLocationDTO();

                GenericMapper.Map(addressLocationDTO, objAddressLocation);

                // return context.Students.Find(id);
                return addressLocationDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> SaveNewAddressLocation(AddressLocationDTO addressLocationDTO)
        {
            try
            {
                var addressLocationEntity = new AddressLocation();

                GenericMapper.Map(addressLocationDTO, addressLocationEntity);

                DataContext.AddressLocations.Add(addressLocationEntity);

                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}