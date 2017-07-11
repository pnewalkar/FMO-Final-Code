namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    using System.Data.Entity;

    public interface IDatabaseFactory<TContext>
        where TContext : DbContext
    {
        TContext Get();
    }
}