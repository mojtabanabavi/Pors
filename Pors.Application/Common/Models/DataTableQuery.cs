using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class DataTableQuery
    {
        public int Page { get; set; }
        public string Draw { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

        public DataTableQuery()
        {
        }

        public DataTableQuery(DataTableQuery query)
        {
            Page = query.Page;
            Draw = query.Draw;
            Search = query.Search;
            PageSize = query.PageSize;
            SortColumn = query.SortColumn;
            SortDirection = query.SortDirection;
        }
    }
}