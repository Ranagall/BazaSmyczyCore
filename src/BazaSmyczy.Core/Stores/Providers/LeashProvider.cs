using BazaSmyczy.Core.Stores.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BazaSmyczy.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BazaSmyczy.Core.Stores.Providers
{
    internal class LeashProvider : ILeashProvider
    {
        private readonly LeashDbContext _context;
        private readonly ILogger<LeashProvider> _logger;

        public LeashProvider(LeashDbContext context, ILogger<LeashProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddLeashAsync(Leash leash)
        {
            _context.Add(leash);
            await SaveChangesAsync();
        }

        public Task<Leash> GetLeashAsync(int id)
        {
            return _context.Leashes.SingleOrDefaultAsync(leash => leash.ID == id);
        }

        public Task<IQueryable<Leash>> GetLeashesAsync()
        {
            var leashes = from leash in _context.Leashes
                          select leash;

            return Task.FromResult(leashes);
        }

        public async Task RemoveLeashAsync(Leash leash)
        {
            _context.Leashes.Remove(leash);
            await SaveChangesAsync();
        }

        public async Task UpdateLeashAsync(Leash leash)
        {
            _context.Update(leash);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
