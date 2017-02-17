using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IUploadManager
    {
        /// <summary>
        ///  Save's file in given path with new created name.
        /// </summary>
        /// <param name="file">File to save</param>
        /// <param name="path">Save path</param>
        /// <returns>New file name or null if couldn't save file.</returns>
        Task<string> SaveFile(IFormFile file, string path);

        /// <summary>
        /// Replaces file in given path with new created name.
        /// </summary>
        /// <param name="file">New file</param>
        /// <param name="path">Replace path </param>
        /// <param name="oldFileName"></param>
        /// <returns>New file name or null if couldn't replace file.</returns>
        Task<string> ReplaceFile(IFormFile file, string path, string oldFileName);

        void DeleteFileIfExists(string path);
    }
}
