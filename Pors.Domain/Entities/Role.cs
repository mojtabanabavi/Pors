using System;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> Permissions { get; set; }

        public Role()
        {
        }

        public Role(string title)
        {
            Title = title;
        }

        public Role(string title, string description) : this(title)
        {
            Description = description;
        }
    }
}
