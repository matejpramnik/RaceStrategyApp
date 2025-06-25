using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;
using System.Diagnostics;

namespace RaceStrategyApp.Controllers {
    public class RaceController : BaseController {

        public virtual IActionResult All() {
            List<Race> races = Ctx.Races.ToList();
            return View(races);
        }

        public IActionResult NewRace() {
            var race = new Race() { 
                TrackState = trackState.green,
                Damage = false,
                TerminalDamage = false,
                LapCount = 0,
                TrackWeather = weather.dry   //VYRIESIT WEATHER
            };
            race.SelectedTyres.Add(tyreCompound.generic);
            return View(race);
        }

        [HttpPost]
        public IActionResult NewRace(Race race) {
            race.AvailableTyres = race.SelectedTyres
                .Where(t => t != null)
                .Select(t => new Tyre { Compound = t })
                .ToList();

            if (ModelState.IsValid) {
            Ctx.Races.Add(race);
            Ctx.SaveChanges();
            return RedirectToAction("Race", "Race", new { id = race.Id });
        }
            return View(race);
        }

        public IActionResult Race(int? id) {
            if (id == null) return NotFound();
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();
            return View(race);
        }
    }
}
