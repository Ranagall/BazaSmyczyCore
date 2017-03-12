using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Stores.Providers;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Stores
{
    internal class LeashesStore : ILeashesStore
    {
        private readonly ILeashProvider _provider;

        public LeashesStore(ILeashProvider provider)
        {
            _provider = provider;
        }

        public Task AddLeashAsync(Leash leash)
        {
            return _provider.AddLeashAsync(leash);
        }

        public Task<Leash> GetLeashAsync(int id)
        {
            return _provider.GetLeashAsync(id);
        }

        public Task<IQueryable<Leash>> GetLeashesAsync()
        {
            return _provider.GetLeashesAsync();
        }

        public Task RemoveLeashAsync(Leash leash)
        {
            return _provider.RemoveLeashAsync(leash);
        }

        public Task UpdateLeashAsync(Leash leash)
        {
            return _provider.UpdateLeashAsync(leash);
        }
    }
}
