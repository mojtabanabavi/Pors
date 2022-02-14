using System;
using System.Collections.Generic;

namespace Pors.Application.Common.Interfaces
{
    public interface IPasswordHashService
    {
        string Hash(string value);
        bool Verify(string providedValue, string hashedValue);
    }
}
