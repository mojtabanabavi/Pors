using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    public class FileManagerService : IFileManagerService
    {
        private const string UPLOAD_PATH = @"wwwroot\uploads\";

        public FileManagerService()
        {
            if (!Directory.Exists(UPLOAD_PATH))
            {
                Directory.CreateDirectory(UPLOAD_PATH);
            }
        }

        public async Task<string> CreateFileAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var imageName = Guid.NewGuid().ToString() + extension;
            var imagePath = Path.Combine(UPLOAD_PATH, imageName);

            try
            {
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return imagePath.Replace("wwwroot", string.Empty);
            }
            catch
            {
                return null;
            }
        }

        public async Task DeleteFileAsync(string path)
        {
            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
        }
    }
}
