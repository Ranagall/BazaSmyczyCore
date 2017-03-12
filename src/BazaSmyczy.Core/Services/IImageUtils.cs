using ImageSharp;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IImageUtils
    {
        Task<Image> PrepareImageAsync(IFormFile file);
        bool IsValidImage(IFormFile file);
        Image ResizeBiggerToFit(Image image, int toFitWidth, int toFitHeight);
    }
}
