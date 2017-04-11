using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fmo.Common.AsyncEnumerator
{
    public class DbAsyncQueryProvider<T> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _localQueryProvider;

        internal  DbAsyncQueryProvider(IQueryProvider localQueryProvider)
        {
            _localQueryProvider = localQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new  DbAsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new  DbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _localQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _localQueryProvider.Execute<TResult>(expression);
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
