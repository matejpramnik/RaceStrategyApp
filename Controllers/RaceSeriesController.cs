using Microsoft.AspNetCore.Mvc;
using RaceStrategyApp.Models;

namespace RaceStrategyApp.Controllers {
    public class RaceSeriesController : BaseController {

        public async virtual Task<IActionResult> All() {
            var rs = await Cont.RaceSeries.ExecuteAsync();
            List<RaceSeries> retval = new List<RaceSeries>();

            foreach (var ras in rs) {
                retval.Add(ras);
            }

            return View(retval);
        }

        public IActionResult NewRaceSeries() {
            RaceSeries raceSeries = new RaceSeries() { 
                Races = new List<Race>()
            };
            return View(raceSeries);
        }

        [HttpPost]
        public IActionResult NewRaceSeries(RaceSeries raceSeries) {
            if (ModelState.IsValid) {
                Ctx.RaceSeries.Add(raceSeries);
                Ctx.SaveChanges();
                return RedirectToAction("RaceSeries", "RaceSeries", new { id = raceSeries.Id });
            }
            return View(raceSeries);
        }

        public IActionResult RaceSeries(int? id) {
            if (id == null) return NotFound();
            var raceSeries = Ctx.RaceSeries.FirstOrDefault(rs => rs.Id == id);
            if (raceSeries == null) return NotFound();
            return View(raceSeries);
        }
    }
}
