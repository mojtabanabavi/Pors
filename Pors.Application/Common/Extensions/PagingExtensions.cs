using System;
using Loby.Tools;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pors.Application.Common.Mappings
{
    public static class PagingExtensions
    {
        public async static Task<PagingResult<T>> ApplyPagingAsync<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            return await Task.Run(() => Paginator.ApplyPaging(queryable, page, pageSize));
        }

        public async static Task<PagingResult<T>> ApplyDataTablePagingAsync<T>(this IQueryable<T> queryable, int skip, int take)
        {
            int totalItems = await queryable.CountAsync();
            var items = await queryable.Skip(skip).Take(take).ToListAsync();

            var result = new PagingResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                TotalFilteredItems = items.Count,
            };

            return result;
        }
    }
}
