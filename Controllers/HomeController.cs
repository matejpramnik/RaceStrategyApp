using Microsoft.AspNetCore.Mvc;

namespace RaceStrategyApp.Controllers {
    public class HomeController : BaseController {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public virtual IActionResult Index() {
            return View();
        }

    }
}