using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VetClinicPublic.Configuration;

namespace VetClinicPublic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SiteConfiguration _siteConfiguration;

        public HomeController(ILogger<HomeController> logger, IOptions<SiteConfiguration> siteConfiguration)
        {
            _siteConfiguration = siteConfiguration.Value;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Hit the Home/Index route");
            ViewBag.PapercutManagementPort = _siteConfiguration.PapercutManagementPort;
            return View();
        }

        public ActionResult TestEmail()
        {
            _logger.LogInformation("Hit the Home/TestEmail route");
            return Ok("Email sent");
        }
    }
}
