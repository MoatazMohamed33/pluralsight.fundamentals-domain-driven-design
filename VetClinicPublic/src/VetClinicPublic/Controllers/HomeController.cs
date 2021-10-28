using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VetClinicPublic.Configuration;
using VetClinicPublic.Interfaces;

namespace VetClinicPublic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISendEmail _mailSender;
        private readonly SiteConfiguration _siteConfiguration;

        public HomeController(ILogger<HomeController> logger, ISendEmail mailSender, IOptions<SiteConfiguration> siteConfiguration)
        {
            _logger = logger;
            _mailSender = mailSender;
            _siteConfiguration = siteConfiguration.Value;
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

            _mailSender.SendEmail(
                to: "test@test.com",
                from: "donotreply@test.com",
                subject: "Test",
                body: "This is just a test.");

            return Ok("Test email has been sent.");
        }
    }
}
