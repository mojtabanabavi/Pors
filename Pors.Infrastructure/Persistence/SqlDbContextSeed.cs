using Pors.Domain.Entities;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Persistence
{
    public static class SqlDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(ISqlDbContext dbContext, IPasswordHashService passwordHasher)
        {
            var administratorRole = new Role("سوپر ادمین");

            if (dbContext.Roles.Any(x => x.Name == administratorRole.Name))
            {
                await dbContext.Roles.AddAsync(administratorRole);
            }

            var administrator = new User
            {
                FirstName = "مجتبی",
                LastName = "نبوی",
                Username = "admin",
                Email = "mj.nabawi@gmail.com",
                PhoneNumber = "09104647055",
                PasswordHash = passwordHasher.Hash("nabavi123344"),
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true
            };

            var administratorUserRole = new UserRole
            {
                UserId = administrator.Id,
                RoleId = administratorRole.Id,
            };

            if (dbContext.Users.Any(u => u.Username == administrator.Username))
            {
                await dbContext.Users.AddAsync(administrator);

                await dbContext.UserRoles.AddAsync(administratorUserRole);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
