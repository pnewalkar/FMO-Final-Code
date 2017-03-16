using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Fmo.DataServices.Infrastructure
{
    public class UnitOfWork<TContext> : Disposable, IUnitOfWork<TContext>
        where TContext : DbContext
    {
        private readonly IDatabaseFactory<TContext> databaseFactory;
        private TContext dataContext;

        public UnitOfWork(IDatabaseFactory<TContext> databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected TContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }
}
