using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
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
            var sortColumnDirection = form["order[0][dir]"].FirstOrDefault();

            // search value from (search box) 
            var searchValue = form["search[value]"].FirstOrDefault();

            start = start == "0" ? "1" : start;
            int page = start != null ? Convert.ToInt32(start) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 15;

            var result = new DataTableQuery
            {
                Draw = draw,
                Page = page,
                PageSize = pageSize,
                SortColumn = sortColumn,
                Search = searchValue.ToLower(),
                SortColumnDirection = sortColumnDirection
            };

            return result;
        }
    }
}