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

        [EnableQuery]
        public SingleResult<Models.RaceProgress> GetProgress([FromODataUri] int key) {
            IQueryable<Models.RaceProgress> result = Ctx.RaceProgresses.Where(rp => rp.RaceId == key);
            return SingleResult.Create(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Models.Race race) {
            Ctx.Races.Add(race);
            Ctx.SaveChanges();

            var key = race.Id;
            var locationUri = $"{Request.Scheme}://{Request.Host}/api/Race({key})";

            return Created(locationUri, race);
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
