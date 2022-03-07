using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Interfaces
{
    public interface ISqlDbContextSeed
    {
        Task SeedDataAsync();
    }
}
