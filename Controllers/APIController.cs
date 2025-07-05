using Microsoft.AspNetCore.Mvc;
using RaceStrategyApp.Models;

namespace RaceStrategyApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class APIController : ControllerBase {
        protected RaceStrategyContext Ctx { get; set; }
        public APIController() {
            Ctx = new RaceStrategyContext();
        }


       

    }
}
