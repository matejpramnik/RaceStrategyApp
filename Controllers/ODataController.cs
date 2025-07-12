using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
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

        // z nejakeho dovodu tu musim dat manualne [HttpGet] atribut, inac to nebude fungovat (404); vo vsetkych ostatnych
        //  controlleroch a metodach, ak tam nie je atribut, je to automaticky GET metoda, okrem tejto, neviem preco;
        //  3 hodiny som to debuggoval a hladal chybu

        //  NEODSTRANOVAT [HttpGet] atribut !!!!!!!!!
        [EnableQuery]
        [HttpGet]
        public IActionResult Get() {
            return Ok(Ctx.Races);
        }


        // Vzdy vracia error 404, neviem na to prist, takze som to iba zakomentoval

        //[EnableQuery]
        //[HttpGet("({key})")]
        //public IActionResult Get([FromODataUri] int key) {
        //    var result = Ctx.Races.Where(r => r.Id == key);
        //    return !result.Any() ? NotFound() : Ok(SingleResult.Create(result));
        //}

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


    [Route("api/RaceProgress")]
    [AllowAnonymous]
    public class ODRaceProgressController : ODataController {
        protected RaceStrategyContext Ctx { get; set; }
        public ODRaceProgressController() {
            Ctx = new RaceStrategyContext();
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get() {
            return Ok(Ctx.RaceProgresses);
        }

        //[EnableQuery]
        //public SingleResult<Models.RaceProgress> Get([FromODataUri] int key) {
        //    IQueryable<Models.RaceProgress> result = Ctx.RaceProgresses.Where(r => r.RaceId == key);
        //    return SingleResult.Create(result);
        //}

        [HttpPost]
        public IActionResult Post([FromBody] Models.RaceProgress raceProgress) {
            Ctx.RaceProgresses.Add(raceProgress);
            Ctx.SaveChanges();

            var key = raceProgress.Id;
            var locationUri = $"{Request.Scheme}://{Request.Host}/api/RaceProgress({key})";

            return Created(locationUri, raceProgress);
        }

        //[HttpPatch]
        //public IActionResult Patch([FromODataUri] int key, Delta<Models.RaceProgress> patch) {
        //    var progress = Ctx.RaceProgresses.FirstOrDefault(r => r.Id == key);
        //    if (progress == null) return NotFound();

        //    patch.Patch(progress);
        //    Ctx.SaveChanges();
        //    return Updated(progress);
        //}

        //[HttpPut]
        //public IActionResult Put([FromODataUri] int key, Models.RaceProgress update) {
        //    if (key != update.Id) return BadRequest();

        //    Ctx.Entry(update).State = EntityState.Modified;
        //    Ctx.SaveChanges();
        //    return Updated(update);
        //}
    }
}
