namespace Fmo.DataServices.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;

    public interface IUnitOfWork<TContext> : IDisposable
        where TContext : DbContext
    {
        void Commit();
    }
}
