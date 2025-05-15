using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RaceStrategyApp.Models;

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