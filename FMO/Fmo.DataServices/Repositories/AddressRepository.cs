using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Fmo.DataServices.Repositories.Interface;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Entities;
using Fmo.DataServices.Repositories.Interfaces;

namespace Fmo.DataServices.Repositories
{
    public class AddressRepository : RepositoryBase<PostalAddress, FMODBContext>, IAddressRepository
    {


        public AddressRepository(IDatabaseFactory<FMODBContext> databaseFactory) : base(databaseFactory)
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
