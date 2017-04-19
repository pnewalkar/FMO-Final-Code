using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fmo.Common.AsyncEnumerator
{
    /// <summary>
    /// This class is used for the NUnit testing of async methods
    /// </summary>
    /// <typeparam name="T"> Generic parameter </typeparam>
    public class DbAsyncQueryProvider<T> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider localQueryProvider;

        internal DbAsyncQueryProvider(IQueryProvider localQueryProvider)
        {
            this.localQueryProvider = localQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new DbAsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new DbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return localQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return localQueryProvider.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
}
