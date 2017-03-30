namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;

    public class AddressLocationRepository : RepositoryBase<AddressLocation, FMODBContext>, IAddressLocationRepository
    {
        public AddressLocationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public AddressLocation GetAddressLocationByUDPRN(int uDPRN)
        {
            try
            {
                var objAddressLocation = DataContext.AddressLocations.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                // return context.Students.Find(id);
                return objAddressLocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}