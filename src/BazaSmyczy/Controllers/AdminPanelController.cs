using BazaSmyczy.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BazaSmyczy.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class AdminPanelController : Controller
    {
        private readonly ILogger<AdminPanelController> _logger;

        public AdminPanelController(ILogger<AdminPanelController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
