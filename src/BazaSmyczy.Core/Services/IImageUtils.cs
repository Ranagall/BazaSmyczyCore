using ImageSharp;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IImageUtils
    {
        Task<Image> PrepareImage(IFormFile file);
        bool IsValidImage(IFormFile file);
        Image ResizeBiggerToFit(Image image, int toFitWidth, int toFitHeight);
    }
}
