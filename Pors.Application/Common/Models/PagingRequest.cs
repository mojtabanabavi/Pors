using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class PagingRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}