using System.Linq.Expressions;
using GAMF.Web.Models;

namespace GAMF.Web.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> AddOrderBy<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> keySelector, OrderDirection orderDirection)
        {
            if (typeof(IOrderedQueryable).IsAssignableFrom(query.Expression.Type) && query is IOrderedQueryable<T> orderedQuery)
            {
                return orderDirection switch
                {
                    OrderDirection.Desc => orderedQuery.ThenByDescending(keySelector),
                    _ => orderedQuery.ThenBy(keySelector)
                };
            }
            else
            {
                return orderDirection switch
                {
                    OrderDirection.Desc => query.OrderByDescending(keySelector),
                    _ => query.OrderBy(keySelector)
                };
            }
        }
    }
}
