namespace Fmo.DataServices.Repositories
{
    using System;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;

    public class AddressRepository : RepositoryBase<PostalAddress, FMODBContext>, IAddressRepository
    {
        public AddressRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool SaveAddress(PostalAddress objPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                DataContext.PostalAddresses.Add(objPostalAddress);
                saveFlag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return saveFlag;
        }
    }
}