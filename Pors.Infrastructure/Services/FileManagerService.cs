using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    public class FileManagerService : IFileManagerService
    {
        private const string ROOT_PATH = "wwwroot";
        private const string UPLOAD_PATH = ROOT_PATH + "/uploads/";

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

                return BuildRelativePath(imagePath);
            }
            catch
            {
                return null;
            }
        }

        public async Task DeleteFileAsync(string path)
        {
            path = Path.Combine(ROOT_PATH, path);

            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
        }

        private string BuildRelativePath(string absolutePath)
        {
            return absolutePath.Replace($"{ROOT_PATH}\\", string.Empty);
        }
    }
}
