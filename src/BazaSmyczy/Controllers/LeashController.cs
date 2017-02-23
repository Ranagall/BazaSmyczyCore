using BazaSmyczy.Core.Extensions;
using BazaSmyczy.Core.Services;
using BazaSmyczy.Data;
using BazaSmyczy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.Controllers
{
    [Authorize]
    public class LeashController : Controller
    {
        private readonly LeashDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly IUploadManager _uploadManager;

        public LeashController(LeashDbContext context, IHostingEnvironment environment, IUploadManager uploadManager)
        {
            _context = context;
            _environment = environment;
            _uploadManager = uploadManager;
        }

        // GET: Leashes
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leashes.ToListAsync());
        }

        // GET: Leashes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leash = await _context.Leashes.SingleOrDefaultAsync(m => m.ID == id);
            if (leash == null)
            {
                return NotFound();
            }

            return View(leash);
        }

        // GET: Leashes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leashes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Color,Desc,Size,Text")] Leash leash)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files["picture"];

                var newFileName = await _uploadManager.SaveFile(file, GetUploadsPath());

                if (!newFileName.IsNullOrEmpty())
                {
                    leash.ImageName = newFileName;

                    _context.Add(leash);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(leash);
        }

        // GET: Leashes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leash = await _context.Leashes.SingleOrDefaultAsync(m => m.ID == id);
            if (leash == null)
            {
                return NotFound();
            }
            return View(leash);
        }

        // POST: Leashes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Color,Desc,ImageName,Size,Text")] Leash leash)
        {
            if (id != leash.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Form.Files["picture"];

                    var newImageName = await _uploadManager.ReplaceFile(file, GetUploadsPath(), leash.ImageName);

                    if (!newImageName.IsNullOrEmpty())
                    {
                        leash.ImageName = newImageName;
                    }

                    _context.Update(leash);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeashExists(leash.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(leash);
        }

        // GET: Leashes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leash = await _context.Leashes.SingleOrDefaultAsync(m => m.ID == id);
            if (leash == null)
            {
                return NotFound();
            }

            return View(leash);
        }

        // POST: Leashes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leash = await _context.Leashes.SingleOrDefaultAsync(m => m.ID == id);

            var oldImagePath = Path.Combine(GetUploadsPath(), leash.ImageName ?? "");
            _uploadManager.DeleteFileIfExists(oldImagePath);

            _context.Leashes.Remove(leash);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Leashes/ShowImage/5
        [AllowAnonymous]
        public async Task<IActionResult> ShowImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leash = await _context.Leashes.SingleOrDefaultAsync(m => m.ID == id);
            if (leash == null)
            {
                return NotFound();
            }

            return PartialView(leash);
        }

        private bool LeashExists(int id)
        {
            return _context.Leashes.Any(e => e.ID == id);
        }

        private string GetUploadsPath()
        {
            return Path.Combine(_environment.WebRootPath, "uploads\\leashes");
        }
    }
}
