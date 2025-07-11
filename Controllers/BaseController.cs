using Microsoft.AspNetCore.Mvc;
using RaceStrategyApp.Models;
using RaceStrategyApp.ODataClient;
using Default;

namespace RaceStrategyApp.Controllers {
    public class BaseController : Controller {
        protected RaceStrategyContext Ctx { get; set; }
        public BaseController() {
            Ctx = new RaceStrategyContext();
        }
        public Container Cont = new Container(new Uri("https://localhost:7044/api"));
    }
}
