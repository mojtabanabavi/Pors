using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    public class DataTableService : IDataTableService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataTableService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DataTableQuery FetchRequest()
        {
            var form = _httpContextAccessor.HttpContext.Request.Form;

            var draw = form["draw"].FirstOrDefault();

            // skip number of rows count
            var start = form["start"].FirstOrDefault();

            // paging length
            var length = form["length"].FirstOrDefault();

            // sort column name
            var sortColumn = form["columns[" + form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            // sort column direction (asc, desc) 
            var sortDirection = form["order[0][dir]"].FirstOrDefault();

            // search value from (search box) 
            var searchKey = form["search[value]"].FirstOrDefault();

            start = start == "0" ? "1" : start;
            int skip = start != null ? Convert.ToInt32(start) : 1;
            int take = length != null ? Convert.ToInt32(length) : 15;

            var result = new DataTableQuery
            {
                Draw = draw,
                Skip = skip,
                Take = take,
                SortColumn = sortColumn,
                Search = searchKey.ToLower(),
                SortDirection = sortDirection
            };

            return result;
        }
    }
}