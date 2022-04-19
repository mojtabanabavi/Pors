using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class ActionInfo
    {
        public string Name { get; set; }
        public bool IsSecured { get; set; }
        public string DisplayName { get; set; }
        public string ControllerId { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string Id => $"{ControllerId}:{Name}".ToLower();
    }
}