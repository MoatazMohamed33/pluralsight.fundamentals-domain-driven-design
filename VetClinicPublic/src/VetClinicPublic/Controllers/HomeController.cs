using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VetClinicPublic.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      _logger.LogInformation("Hit the Home/Index route");
      return View();
    }

    public ActionResult TestEmail()
    {
      _logger.LogInformation("Hit the Home/TestEmail route");
      return Ok("Email sent");
    }
  }
}
