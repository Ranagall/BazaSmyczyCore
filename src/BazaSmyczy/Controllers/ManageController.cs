using BazaSmyczy.Core.Models.Results;
using BazaSmyczy.Core.Services;
using BazaSmyczy.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BazaSmyczy.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IManageService _manageService;

        public ManageController(IManageService manageService)
        {
            _manageService = manageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : "";

            var user = await _manageService.GetCurrentUserAsync(HttpContext.User);
            if (user == null)
            {
                return View("Error");
            }

            var model = new IndexViewModel
            {
                HasPassword = await _manageService.HasPasswordAsync(user),
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _manageService.ChangePasswordAsync(HttpContext.User, model.OldPassword, model.NewPassword);

            if (!result.IsError)
            {
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);
            return View(model);
        }

        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _manageService.SetPasswordAsync(HttpContext.User, model.NewPassword);

            if (!result.IsError)
            {
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
            }

            AddErrors(result);
            return View(model);
        }

        #region Helpers

        private void AddErrors(Result result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess
        }

        #endregion
    }
}
