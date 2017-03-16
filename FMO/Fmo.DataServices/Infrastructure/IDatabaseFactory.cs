using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Fmo.DataServices.Infrastructure
{
    public interface IDatabaseFactory<TContext> : IDisposable
        where TContext : DbContext
    {
        TContext Get();
    }
}
