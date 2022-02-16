using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Pors.Application.Common.Interfaces
{
    public interface IFileManagerService
    {
        Task<string> CreateFileAsync(IFormFile file);
    }
}