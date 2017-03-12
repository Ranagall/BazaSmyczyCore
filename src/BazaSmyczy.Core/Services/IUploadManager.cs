using BazaSmyczy.Core.Models.Results;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IUploadManager
    {
        Task<FileResult> SaveFileAsync(IFormFile file);

        Task<FileResult> ReplaceFileAsync(IFormFile file, string oldFileName);

        void DeleteFileIfExists(string fileName);
    }
}
