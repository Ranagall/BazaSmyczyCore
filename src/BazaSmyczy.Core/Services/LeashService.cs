using BazaSmyczy.Core.Extensions;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Models.Results;
using BazaSmyczy.Core.Stores;
using BazaSmyczy.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    internal class LeashService : ILeashService
    {
        private readonly ILeashesStore _store;
        private readonly IUploadManager _uploadManager;
        private readonly ILogger<LeashService> _logger;

        public LeashService(ILeashesStore store, IUploadManager uploadManager, ILogger<LeashService> logger)
        {
            _store = store;
            _uploadManager = uploadManager;
            _logger = logger;
        }

        public Task<Leash> GetLeashAsync(int id)
        {
            return _store.GetLeashAsync(id);
        }

        public async Task<Result> CreateLeashAsync(Leash leash, IFormFile file)
        {
            var result = await _uploadManager.SaveFileAsync(file);

            if (!result.IsError)
            {
                leash.ImageName = result.NewFileName;
                leash.Color = leash.Color.ToTitleCase();
                leash.CreationTime = DateTime.Now;

                await _store.AddLeashAsync(leash);
            }

            return result;
        }
      
        public async Task DeleteLeashAsync(int id)
        {
            var leash = await GetLeashAsync(id);

            _uploadManager.DeleteFileIfExists(leash.ImageName);

            await _store.RemoveLeashAsync(leash);
        }

        public async Task<Result> EditLeashAsync(Leash leash, IFormFile file)
        {
            var result = new Result();
            try
            {
                if (!file.IsNullOrEmpty())
                {
                    var fileResult = await _uploadManager.ReplaceFileAsync(file, leash.ImageName);

                    if (!fileResult.IsError)
                    {
                        leash.ImageName = fileResult.NewFileName;
                    }
                    else
                    {
                        return fileResult;
                    }
                }

                leash.Color = leash.Color.ToTitleCase();
                await _store.UpdateLeashAsync(leash);
                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _store.GetLeashAsync(leash.ID) == null)
                {
                    result.Errors.Add("Leash doesn't exist");
                }
                else
                {
                    throw;
                }
            }
            return result;
        }

        public async Task<PaginatedList<Leash>> GetLeashesAsync(PageCriteria pageCriteria)
        {
            var searchFilter = pageCriteria.search;

            var leashes = await _store.GetLeashesAsync();

            leashes = leashes.OrderByDescending(z => z.CreationTime);

            if (!searchFilter.IsNullOrEmpty())
            {
                leashes = leashes.Where(leash =>
                    leash.Text.ToLower().Contains(searchFilter.ToLower())
                    || leash.Desc.ToLower().Contains(searchFilter.ToLower()));
            }

            return await PaginatedList<Leash>.CreateAsync(leashes.AsNoTracking(), pageCriteria.page, pageCriteria.pageSize);
        }
    }
}
