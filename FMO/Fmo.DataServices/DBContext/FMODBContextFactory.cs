namespace Fmo.DataServices.DBContext
{
    using System.Configuration;
    using System.Data.Entity.Infrastructure;

    public class FMODBContextFactory : IDbContextFactory<FMODBContext>
    {
        public FMODBContext Create()
        {
            return new FMODBContext(ConfigurationManager.ConnectionStrings["FMODBContext"].ToString());
        }
    }
}