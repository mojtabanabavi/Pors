using System;
using System.Linq;
using System.Text;
using Pors.Domain.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    public class TokenBuilderService : ITokenBuilderService
    {
        private readonly Random _random;
        private readonly ISqlDbContext _dbContext;

        public TokenBuilderService(ISqlDbContext dbContext)
        {
            _random = new Random();
            _dbContext = dbContext;
        }

        public async Task<string> CreateTokenAsync(int userId, IdentityTokenType tokenType, IdentityTokenDataType dataType)
        {
            var token = string.Empty;

            do
            {
                if (dataType == IdentityTokenDataType.Numerical)
                {
                    token = _random.Next(1000, 9999).ToString();
                }
                else
                {
                    token = Guid.NewGuid().ToString();
                }
            }
            while (await IsTokenExistAsync(userId, token));

            return token;
        }

        private async Task<bool> IsTokenExistAsync(int userId, string token)
        {
            return await _dbContext.UserTokens.AnyAsync(x => x.UserId == userId && x.Value == token);
        }
    }
}
