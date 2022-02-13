using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pors.Domain.Enums
{
    public enum IdentityTokenType
    {
        ResetPassword = 3,
        EmailVerification = 4,
        PhoneNumberVerification = 5
    }
}
