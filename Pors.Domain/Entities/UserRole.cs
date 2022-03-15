using System;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }

        public UserRole()
        {
        }

        public UserRole(int roleId)
        {
            RoleId = roleId;
        }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
