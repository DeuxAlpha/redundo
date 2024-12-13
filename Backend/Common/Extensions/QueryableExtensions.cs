using System.Linq;

namespace Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> PaginateItems<TEntity>(this IQueryable<TEntity> items, int page, int numberOfItems)
        {
            return items.Skip(numberOfItems * (page - 1)).Take(numberOfItems);
        }
    }
}