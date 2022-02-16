using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Models;

namespace Pors.Application.Common.Mappings
{
    public static class PagingExtensions
    {
        public static async Task<PagingResult<T>> ApplyPagingAsync<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            return await PagingResult<T>.ApplyPaging(queryable, page, pageSize);
        }
    }
}
