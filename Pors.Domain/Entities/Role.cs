using System;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
