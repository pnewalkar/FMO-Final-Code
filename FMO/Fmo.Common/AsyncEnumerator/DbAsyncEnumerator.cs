using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fmo.Common.AsyncEnumerator
{
    public class DbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _localEnumerator;

        public  DbAsyncEnumerator(IEnumerator<T> localEnumerator)
        {
            _localEnumerator = localEnumerator;
        }

        public void Dispose()
        {
            _localEnumerator.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_localEnumerator.MoveNext());
        }

        public T Current
        {
            get { return _localEnumerator.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}
