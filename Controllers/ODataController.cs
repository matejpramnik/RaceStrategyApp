using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RaceStrategyApp.Models;
using RaceStrategyApp.ODataClient;

namespace RaceStrategyApp.Controllers {

    [Route("api/Race")]
    [AllowAnonymous]
    public class ODRaceController : ODataController {
        protected RaceStrategyContext Ctx { get; set; }
        public ODRaceController() {
            Ctx = new RaceStrategyContext();
        }

        [EnableQuery]
        public IActionResult GetAll() {
            return Ok(Ctx.Races);
        }

        [EnableQuery]
        public SingleResult<Models.Race> Get([FromODataUri] int key) {
            IQueryable<Models.Race> result = Ctx.Races.Where(r => r.Id == key);
            return SingleResult.Create(result);
        }
    }


    [Route("api/RaceSeries")]
    [AllowAnonymous]
    public class ODRaceSeriesController : ODataController {
        protected RaceStrategyContext Ctx { get; set; }
        public ODRaceSeriesController() {
            Ctx = new RaceStrategyContext();
        }

        [EnableQuery]
        public IActionResult GetAll() {
            return Ok(Ctx.RaceSeries);
        }

        [EnableQuery]
        public SingleResult<Models.RaceSeries> Get([FromODataUri] int key) {
            IQueryable<Models.RaceSeries> result = Ctx.RaceSeries.Where(r => r.Id == key);
            return SingleResult.Create(result);
        }
    }
}
