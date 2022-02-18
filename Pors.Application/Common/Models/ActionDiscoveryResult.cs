using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class ActionDiscoveryResult
    {
        public string Area { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ActionDisplayName { get; set; }
    }
}
