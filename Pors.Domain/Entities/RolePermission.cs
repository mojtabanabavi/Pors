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
        public string Action { get; set; }
        public string Controller { get; set; }
        public DateTime CreatedAt { get; set; }

        public Role Role { get; set; }

        public RolePermission()
        {
        }

        public RolePermission(string controller, string action)
        {
            Action = action;
            Controller = controller;
        }

        public RolePermission(int roleId, string controller, string action) : this(controller, action)
        {
            RoleId = roleId;
        }
    }
}