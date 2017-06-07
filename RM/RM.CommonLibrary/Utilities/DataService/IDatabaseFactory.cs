namespace RM.CommonLibrary.DataMiddleware
{
    using System.Data.Entity;

    public interface IDatabaseFactory<TContext>
        where TContext : DbContext
    {
        TContext Get();
    }
}