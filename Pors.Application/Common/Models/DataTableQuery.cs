using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class DataTableQuery
    {
        public int Skip { get; set; }
        public string Draw { get; set; }
        public int Take { get; set; }
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

        public DataTableQuery()
        {
        }

        public DataTableQuery(DataTableQuery query)
        {
            Skip = query.Skip;
            Draw = query.Draw;
            Take = query.Take;
            Search = query.Search;
            SortColumn = query.SortColumn;
            SortDirection = query.SortDirection;
        }
    }
}