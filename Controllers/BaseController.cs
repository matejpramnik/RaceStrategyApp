using Microsoft.AspNetCore.Mvc;
using RaceStrategyApp.Models;

namespace RaceStrategyApp.Controllers {
    public class BaseController : Controller {
        protected RaceStrategyContext Ctx { get; set; }
        public BaseController() {
            Ctx = new RaceStrategyContext();
        }
    }
}
