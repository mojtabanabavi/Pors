using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pors.Application.Common.Models
{
    public class ControllerInfo
    {
        public string Name { get; set; }
        public bool IsSecured { get; set; }
        public string AreaName { get; set; }
        public string DisplayName { get; set; }
        public List<ActionInfo> Actions { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string Id => $"{AreaName}:{Name}".ToLower();
    }
}
