using BazaSmyczy.Core.Configs;
using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Extensions;
using BazaSmyczy.Core.Models.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public class UploadManager : IUploadManager
    {
        private readonly IImageUtils _imageUtils;
        private readonly ILogger<UploadManager> _logger;
        private readonly IHostingEnvironment _environment;
        private readonly BazaSmyczyOptions _options;

        public UploadManager(IImageUtils imageUtils, ILogger<UploadManager> logger, IHostingEnvironment env, IOptions<BazaSmyczyOptions> options)
        {
            _imageUtils = imageUtils;
            _logger = logger;
            _environment = env;
            _options = options.Value;
        }

        public async Task<FileResult> SaveFileAsync(IFormFile file)
        {
            var path = GetUploadsPath();
            var result = new FileResult();

            if (!file.IsNullOrEmpty())
            {
                if (_imageUtils.IsValidImage(file))
                {
                    var newFileName = CreateUniqueName(file);
                    var image = await _imageUtils.PrepareImageAsync(file);

                    using (var fileStream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
                    {
                        image.Save(fileStream);
                        _logger.LogInformation(EventsIds.File.Saved, $"Image \"{newFileName}\" uploaded successfully");
                    }

                    result.NewFileName = newFileName;
                    return result;
                }
            }

            result.Errors.Add("Invalid image");
            _logger.LogWarning(EventsIds.File.SaveFailed, $"Coulnd't save file \"{file?.FileName}\" to {path}");
            return result;
        }

        public async Task<FileResult> ReplaceFileAsync(IFormFile file, string oldFileName)
        {
            var result = await SaveFileAsync(file);

            if (!result.IsError)
            {
                DeleteFileIfExists(Path.Combine(GetUploadsPath(), oldFileName));
            }

            return result;
        }

        public void DeleteFileIfExists(string fileName)
        {
            var path = Path.Combine(GetUploadsPath(), fileName ?? string.Empty);

            if (File.Exists(path))
            {
                File.Delete(path);
                _logger.LogInformation(EventsIds.File.Deleted, "Deleted file successfully");
            }
        }

        private string CreateUniqueName(IFormFile file)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        }

        private string GetUploadsPath()
        {
            return Path.Combine(_environment.WebRootPath, _options.UploadsPath);
        }
    }
}
