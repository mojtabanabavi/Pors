using Loby.Tools;
using System.Linq;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Persistence
{
    public static class SqlDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(SqlDbContext dbContext)
        {
            var administratorRoleName = "سوپر ادمین";

            var administrator = new User
            {
                FirstName = "مجتبی",
                LastName = "نبوی",
                Email = "mj.nabawi@gmail.com",
                PhoneNumber = "09104647055",
                PasswordHash = PasswordHasher.Hash("nabavi123344"),
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                UserRoles = new List<UserRole>
                {
                    new UserRole()
                    {
                        Role = new Role(administratorRoleName)
                    }
                }
            };

            if (!dbContext.Users.Any(u => u.Email == administrator.Email))
            {
                await dbContext.Users.AddAsync(administrator);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
