using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class PostalAddressRepository : RepositoryBase<PostalAddress, FMODBContext>, IPostalAddressRepository
    {
        public PostalAddressRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}