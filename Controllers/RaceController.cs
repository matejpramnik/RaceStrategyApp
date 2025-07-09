using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using RaceStrategyApp.Migrations;

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

            if (id == null) return NotFound();
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            if (Ctx.RaceProgresses.Any(rp => rp.RaceId == id) == true) {
                ViewData["RaceStarted"] = true;

                if (race.LapCount + 1 <= race.NumberOfLaps) {
                    ViewData["LapCount++?"] = true;
                }
                else ViewData["LapCount++?"] = false;

                ViewBag.TrackWeatherList = Enum.GetValues(typeof(weather))
                            .Cast<weather>()
                            .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                            .ToList();

                ViewBag.TrackStateList = Enum.GetValues(typeof(trackState))
                    .Cast<trackState>()
                    .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
                    .ToList();
            }

            return View(race);
        }

        [HttpPost]
        public IActionResult Race(int id) {
            ViewData["RaceStarted"] = true;
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            ViewBag.TrackWeatherList = Enum.GetValues(typeof(weather))
                .Cast<weather>()
                .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                .ToList();

            ViewBag.TrackStateList = Enum.GetValues(typeof(trackState))
                .Cast<trackState>()
                .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
            .ToList();

            if (Ctx.RaceProgresses.FirstOrDefault(rp => rp.RaceId == race.Id) == null) {
                RaceProgress newRace = new RaceProgress() {
                    RaceId = id
                };
                newRace.RaceSnapshots.Add(race);
                Ctx.RaceProgresses.Add(newRace);
                Ctx.SaveChanges();
            }

            return View(race);
        }

        [HttpPost]
        public IActionResult IncrementLapCount(int id) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            ViewData["LapCount++?"] = true;

            if (race.LapCount + 1 <= race.NumberOfLaps) {
                race.LapCount++;
                int res = FindAndSaveProgress(race);
                if (res == -1) return NotFound();
            }
            else {
                ViewData["LapCount++?"] = false;
            }
            
            return RedirectToAction("Race", race);
        }

        [HttpPost]
        public IActionResult SetWeather(int id, weather trackWeather) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.TrackWeather = trackWeather;
            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", race);
        }

        [HttpPost]
        public IActionResult SetState(int id, trackState trackState) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.TrackState = trackState;
            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", race);
        }

        [HttpPost]
        public IActionResult UpdatePosition(int id, int newPos) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            if (race.Position + newPos >= 1) {
                race.Position += newPos;
                int res = FindAndSaveProgress(race);
                if (res == -1) return NotFound();
            }
            
            return RedirectToAction("Race", race);
        }


        private int FindAndSaveProgress(Race race) {
            var raceProgress = Ctx.RaceProgresses.FirstOrDefault(rp => rp.RaceId == race.Id);
            if (raceProgress == null) return -1;
            raceProgress.RaceSnapshots.Add(race);
            Ctx.SaveChanges();
            return 0;
        }
    }
}
