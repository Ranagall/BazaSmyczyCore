using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Services;
using BazaSmyczy.Core.Utils;
using BazaSmyczy.ViewModels.LeashViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BazaSmyczy.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class LeashController : Controller
    {
        private readonly ILeashService _service;

        public LeashController(ILeashService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(PageCriteria pageCriteria)
        {
            ViewData["SearchFilter"] = pageCriteria.search;
            ViewData["ReturnUrl"] = Request.Path + Request.QueryString;

            var list = await _service.GetLeashesAsync(pageCriteria);

            return View(list);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var leash = await _service.GetLeashAsync(id);
            if (leash != null)
            {
                return View(leash);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Color,Desc,Size,Text")] Leash leash)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files["picture"];
                var result = await _service.CreateLeashAsync(leash, file);
                if (!result.IsError)
                {
                    return RedirectToAction("Index");
                }

                AddModelErrors(result.Errors);
            }

            return View(leash);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var leash = await _service.GetLeashAsync(id);
            if (leash != null)
            {
                return View(leash);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Color,Desc,ImageName,Size,Text")] Leash leash, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (id != leash.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var file = Request.Form.Files["picture"];
                var result = await _service.EditLeashAsync(leash, file);
                if (!result.IsError)
                {
                    return RedirectToLocal(returnUrl);
                }

                AddModelErrors(result.Errors);
            }

            return View(leash);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var leash = await _service.GetLeashAsync(id);
            if (leash != null)
            {
                return View(leash);
            }

            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            await _service.DeleteLeashAsync(id);
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShowImage(int id)
        {
            var leash = await _service.GetLeashAsync(id);
            if (leash != null)
            {
                return PartialView(new ShowImageViewModel { ImageName = leash.ImageName });
            }

            return NotFound();
        }

        private void AddModelErrors(IList<string> list)
        {
            foreach (var error in list)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(LeashController.Index), "Leash");
            }
        }
    }
}
