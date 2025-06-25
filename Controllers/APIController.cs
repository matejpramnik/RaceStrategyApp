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

        [HttpGet("dropdown")]
        public IActionResult GetTyreDropdown() {
            var options = Enum.GetValues(typeof(tyreCompound))
                .Cast<tyreCompound>()
                .Select(t => new {
                    Value = t.ToString(),
                    Text = t.ToString()
                });

            return Ok(options);
        }
    }
}
