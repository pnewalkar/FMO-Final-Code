using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Fmo.Common.AsyncEnumerator
{
    /// <summary>
    /// This class is used for the NUnit testing of async methods
    /// </summary>
    /// <typeparam name="T"> Generic parameter </typeparam>
    public class DbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> localEnumerator;

        public DbAsyncEnumerator(IEnumerator<T> localEnumerator)
        {
            this.localEnumerator = localEnumerator;
        }

        public T Current
        {
            get { return localEnumerator.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            localEnumerator.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(localEnumerator.MoveNext());
        }
    }
}
