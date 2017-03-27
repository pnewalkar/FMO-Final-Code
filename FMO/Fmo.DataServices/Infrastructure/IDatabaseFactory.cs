namespace Fmo.DataServices.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;

    public interface IDatabaseFactory<TContext> : IDisposable
        where TContext : DbContext
    {
        TContext Get();
    }
}
