using System;
using System.Collections.Generic;

namespace Pors.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
