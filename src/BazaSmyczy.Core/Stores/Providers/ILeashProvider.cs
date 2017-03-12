using BazaSmyczy.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Stores.Providers
{
    internal interface ILeashProvider
    {
        Task<Leash> GetLeashAsync(int id);
        Task AddLeashAsync(Leash leash);
        Task UpdateLeashAsync(Leash leash);
        Task RemoveLeashAsync(Leash leash);
        Task<IQueryable<Leash>> GetLeashesAsync();
    }
}
