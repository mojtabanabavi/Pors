using System;
using System.Linq;
using System.Text;
using Pors.Domain.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Interfaces
{
    public interface ITokenBuilderService
    {
        Task<string> CreateTokenAsync(int userId, IdentityTokenType tokenType, IdentityTokenDataType dataType);
    }
}