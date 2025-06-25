using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;
using System.Diagnostics;

namespace RaceStrategyApp.Controllers {
    public class RaceSeriesController : BaseController {

        public virtual IActionResult All() {
            List<RaceSeries> series = Ctx.RaceSeries.ToList();
            return View(series);
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

        public IActionResult RaceSeries() {
            return View();
        }
    }
}
