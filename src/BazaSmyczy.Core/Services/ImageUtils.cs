using BazaSmyczy.Core.Consts;
using ImageSharp;
using ImageSharp.Processing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public class ImageUtils : IImageUtils
    {
        private readonly ILogger<ImageUtils> _logger;

        private const int ImageWidth = 1024;
        private const int ImageHeight = 768;

        public ImageUtils(ILogger<ImageUtils> logger)
        {
            _logger = logger;
        }

        public bool IsValidImage(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!extension.Equals(".jpg") && !extension.Equals(".jpeg") && !extension.Equals(".png"))
            {
                _logger.LogWarning(EventsIds.File.InvalidExt, $"Unwanted file extension: {extension}");
                return false;
            }

            if (!file.ContentType.ToLower().Equals("image/jpg") && !file.ContentType.ToLower().Equals("image/jpeg") && !file.ContentType.ToLower().Equals("image/png"))
            {
                _logger.LogWarning(EventsIds.File.InvalidContentType, $"Unwanted content type: {file.ContentType}");
                return false;
            }

            var jpegBytes = new byte[] { 255, 216, 255 };
            var pngBytes = new byte[] { 137, 80, 78 };
            var reader = new BinaryReader(file.OpenReadStream());
            var fileBytes = reader.ReadBytes(jpegBytes.Length);

            if (!fileBytes.SequenceEqual(jpegBytes) && !fileBytes.SequenceEqual(pngBytes))
            {
                _logger.LogWarning(EventsIds.File.InvalidSignature, $"Unwanted file signature: {fileBytes}");
                return false;
            }

            return true;
        }

        public async Task<Image> PrepareImage(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var image = new Image(stream);

                if (image.Height > image.Width)
                {
                    image.RotateFlip(RotateType.Rotate270, FlipType.None);
                }

                image = ResizeBiggerToFit(image, ImageWidth, ImageHeight);

                return image;
            }
        }

        public Image ResizeBiggerToFit(Image image, int toFitWidth, int toFitHeight)
        {
            var dScaleWidth = GetScaleFactor(image.Width, toFitWidth);
            var dScaleHeight = GetScaleFactor(image.Height, toFitHeight);

            var dScale = Math.Min(dScaleHeight, dScaleWidth);

            if (dScale < 1d)
            {
                var scaleWidth = (int)Math.Round(image.Width * dScale);
                var scaleHeight = (int)Math.Round(image.Height * dScale);

                image.Resize(scaleWidth, scaleHeight);

            }
            return image;
        }

        private double GetScaleFactor(int masterSize, int targetSize)
        {
            return (double)targetSize / masterSize;
        }
    }
}
