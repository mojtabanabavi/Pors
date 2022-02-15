using Pors.Domain.Entities;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Persistence
{
    public static class SqlDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(SqlDbContext dbContext, IPasswordHashService passwordHasher)
        {
            var administratorRoleName = "سوپر ادمین";

            var administrator = new User
            {
                FirstName = "مجتبی",
                LastName = "نبوی",
                Username = "admin",
                Email = "mj.nabawi@gmail.com",
                PhoneNumber = "09104647055",
                PasswordHash = passwordHasher.Hash("nabavi123344"),
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

            if (!dbContext.Users.Any(u => u.Username == administrator.Username))
            {
                await dbContext.Users.AddAsync(administrator);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
