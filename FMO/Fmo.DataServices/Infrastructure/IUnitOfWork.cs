using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Fmo.DataServices.Infrastructure
{
    public interface IUnitOfWork<TContext> : IDisposable
        where TContext : DbContext
    {
        void Commit();
    }
}
