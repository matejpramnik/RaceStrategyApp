using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;

namespace RaceStrategyApp.Controllers {
    public class RaceController : BaseController {

        public virtual IActionResult All() {
            List<Race> races = Ctx.Races.ToList();
            return View(races);
        }

        public IActionResult NewRace() {
            var race = new Race {
                Id = 1,
                Damage = false,
                TerminalDamage = false,
                TrackState = trackState.green,
                RaceSeriesId = 1,
                LapCount = 0
            };
            return View(race);
        }

        [HttpPost]
        public IActionResult NewRace(Race race) {
            race.AvailableTyres = race.SelectedTyres
                .Select(t => new Tyre { Compound = t })
                .ToList();

            Ctx.Races.Add(race);
            Ctx.SaveChanges();

            return RedirectToAction("Race", "Race", new {id = race.Id});
        }

        public IActionResult Race() {
            return View();
        }
    }
}
