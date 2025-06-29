using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

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
            };
            race.SelectedTyres.Add(tyreCompound.generic);

            var raceSeriesList = Ctx.RaceSeries
                .Select(rs => new SelectListItem { Value = rs.Id.ToString(), Text = rs.Name })
                .ToList();
            ViewBag.RaceSeriesList = raceSeriesList;

            ViewBag.TrackWeatherList = Enum.GetValues(typeof(weather))
                .Cast<weather>()
                .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                .ToList();

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
            ViewData["RaceStarted"] = false;

            if (Ctx.RaceProgresses.Any()) {
                if (Ctx.RaceProgresses.Any(rp => rp.RaceId == id) == true) {
                    ViewData["RaceStarted"] = true;
                }
            }

            if (id == null) return NotFound();
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();
            return View(race);
        }

        [HttpPost]
        public IActionResult Race(int id) {
            ViewData["RaceStarted"] = true;
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            RaceProgress newRace = new RaceProgress() {
                RaceId = id
            };
            newRace.RaceSnapshots.Add(race);
            Ctx.RaceProgresses.Add(newRace);
            Ctx.SaveChanges();

            return View(race);

        }
    }
}
