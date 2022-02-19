using System;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
        public byte AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEndAt { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public DateTime RegisterDateTime { get; set; }

        public ICollection<UserToken> Tokens { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
