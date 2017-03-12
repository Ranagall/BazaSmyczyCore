using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Models.Results;
using BazaSmyczy.Core.Utils;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface ILeashService //Todo: find better name
    {
        Task<Leash> GetLeashAsync(int id);
        Task<Result> CreateLeashAsync(Leash leash, IFormFile file);
        Task DeleteLeashAsync(int id);
        Task<Result> EditLeashAsync(Leash leash, IFormFile file);
        Task<PaginatedList<Leash>> GetLeashesAsync(PageCriteria pageCriteria);
    }
}
