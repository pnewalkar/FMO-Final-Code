using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Fmo.DataServices.Entities;
using System.Configuration;

namespace Fmo.DataServices.Infrastructure
{
public class DatabaseFactory<TContext> : Disposable, IDatabaseFactory<TContext>
    where TContext : DbContext
{
    private TContext dataContext;
    public TContext Get()
    {
            return dataContext ?? (dataContext = (TContext)Activator.CreateInstance(typeof(TContext), "Data Source=10.246.8.254 ;Initial Catalog=FMO-AD;User ID=sa;Password=Password1#"));
        }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
