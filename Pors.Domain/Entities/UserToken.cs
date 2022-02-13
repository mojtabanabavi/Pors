using System;
using Pors.Domain.Enums;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class UserToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IdentityTokenType Type { get; set; }
        public string Value { get; set; }
        public DateTime ExpireAt { get; set; }

        public User User { get; set; }
    }
}
