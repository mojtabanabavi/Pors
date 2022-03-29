using System;
using System.IO;
using Loby.Extensions;
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
            var path = GenerateFilePath(file);

            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return path.Replace($"{ROOT_PATH}/", string.Empty);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string path)
        {
            await DeleteFileAsync(path);

            path = GenerateFilePath(file);

            try
            {
                using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    await file.CopyToAsync(fileStream);
                }

                return path.Replace($"{ROOT_PATH}/", string.Empty);
            }
            catch
            {
                return null;
            }
        }

        public async Task DeleteFileAsync(string path)
        {
            path = Path.Combine(ROOT_PATH, path ?? string.Empty);

            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
        }

        private string GenerateFilePath(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var imageName = Guid.NewGuid().ToString() + extension;
            var imagePath = Path.Combine(UPLOAD_PATH, imageName);

            return imagePath;
        }
    }
}
