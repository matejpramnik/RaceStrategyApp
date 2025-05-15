using Microsoft.AspNetCore.Mvc;
using RaceStrategyApp.Models;

namespace RaceStrategyApp.Controllers {
    public class BaseController : ControllerBase {
        protected RaceStrategyContext Ctx { get; set; }
        public BaseController() {
            Ctx = new RaceStrategyContext();
        }
    }
}
