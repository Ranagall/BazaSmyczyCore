using Microsoft.AspNetCore.Http;

namespace BazaSmyczy.Core.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool IsNullOrEmpty(this IFormFile file)
        {
            return file == null || file.Length == 0;
        }
    }
}
