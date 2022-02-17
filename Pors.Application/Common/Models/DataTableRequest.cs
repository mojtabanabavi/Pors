using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class DataTableRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Draw { get; set; }
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
    }
}