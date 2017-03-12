using BazaSmyczy.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazaSmyczy.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
