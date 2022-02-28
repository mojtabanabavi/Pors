using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;

namespace Pors.Application.Common.Mappings
{
    public static class PagingExtensions
    {
        public static async Task<PagingResult<T>> ApplyPagingAsync<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            int totalItems = await queryable.CountAsync();
            var items = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PagingResult<T>
            {
                Items = items,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return result;
        }
    }
}
