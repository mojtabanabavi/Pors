using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class PagingResult
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class PagingResult<T> : PagingResult
    {
        public IEnumerable<T> Items { get; set; }
    }
}