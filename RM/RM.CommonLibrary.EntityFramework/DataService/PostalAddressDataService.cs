using RM.CommonLibrary.EntityFramework.DataService.Interfaces;

using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class PostalAddressDataService : DataServiceBase<PostalAddress, FMODBContext>, IPostalAddressDataService
    {
        public PostalAddressDataService(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}