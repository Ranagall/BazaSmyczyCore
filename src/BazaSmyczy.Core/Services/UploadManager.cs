using BazaSmyczy.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public class UploadManager : IUploadManager
    {
        private readonly IImageUtils _imageUtils;

        public UploadManager(IImageUtils imageUtils)
        {
            _imageUtils = imageUtils;
        }

        public async Task<string> SaveFile(IFormFile file, string path)
        {
            if (!file.IsNullOrEmpty())
            {
                if (_imageUtils.IsValidImage(file))
                {
                    var newFileName = CreateUniqueName(file);
                    var image = await _imageUtils.PrepareImage(file);

                    using (var fileStream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
                    {
                        image.Save(fileStream);
                    }

                    return newFileName;
                }
            }
            return null;
        }

        public async Task<string> ReplaceFile(IFormFile file, string path, string oldFileName)
        {
            var newFileName = await SaveFile(file, path);

            if (!newFileName.IsNullOrEmpty())
            {
                DeleteFileIfExists(Path.Combine(path, oldFileName));
            }

            return newFileName;
        }

        public void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private string CreateUniqueName(IFormFile file)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        }
    }
}
