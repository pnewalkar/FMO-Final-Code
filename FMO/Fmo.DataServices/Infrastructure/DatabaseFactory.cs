using System;
using System.Data.Entity;

namespace Fmo.DataServices.Infrastructure
{
    public class DatabaseFactory<TContext> : Disposable, IDatabaseFactory<TContext>
        where TContext : DbContext
    {
        private TContext dataContext;

        public DatabaseFactory(TContext context)
        {
            dataContext = context;
        }

        public TContext Get()
        {
            return dataContext ?? (dataContext = (TContext)Activator.CreateInstance(typeof(TContext)));
        }

        protected override void DisposeCore()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }
    }
}