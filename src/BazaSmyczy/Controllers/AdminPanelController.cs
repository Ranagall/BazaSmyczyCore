using BazaSmyczy.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BazaSmyczy.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class AdminPanelController : Controller
    {
        private readonly IConfigurationRoot _config;

        public AdminPanelController(IConfigurationRoot config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            ViewData["Version"] = _config.GetValue<string>("Version");
            return View();
        }
    }
}
