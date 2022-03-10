using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }

        public Role Role { get; set; }
    }
}