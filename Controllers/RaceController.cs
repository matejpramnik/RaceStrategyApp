using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

namespace RaceStrategyApp.Controllers {
    public class RaceController : BaseController {

        public async virtual Task<IActionResult> All() {
            var races = await Cont.Race.ExecuteAsync();
            List<Race> retval = new List<Race>();

            foreach (var race in races) {
                retval.Add(race);
            }

            return View(retval);
        }

        [HttpPost]
        public async virtual Task<IActionResult> All(int? RSid) {
            var races = await Cont.Race.ExecuteAsync();
            List<Race> retval = new List<Race>();

            foreach (var race in races) {if (race.RaceSeriesId == RSid) retval.Add(race);
            }

            return View(retval);
        }

        public async Task<IActionResult> NewRace() {
            var race = new Race() {
                TrackState = TrackState.green,
                Damage = false,
                TerminalDamage = false,
                LapCount = 0,
                NumberOfStops = 0,
                LastRefuelLap = 0,
                AmountOfOpponents = 0,
            };
            race.SelectedTyres.Add(TyreCompound.generic);

            var raceSeriesList = await Cont.RaceSeries.
                Select(rs => new SelectListItem { Value = rs.Id.ToString(), Text = rs.Name })
                .ToListAsync();

            var araceSeriesList = Ctx.RaceSeries
                .Select(rs => new SelectListItem { Value = rs.Id.ToString(), Text = rs.Name })
                .ToList();
            ViewBag.RaceSeriesList = raceSeriesList;

            ViewBag.TrackWeatherList = Enum.GetValues(typeof(Weather))
                .Cast<Weather>()
                .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                .ToList();

            return View(race);
        }

        [HttpPost]
        public IActionResult NewRace(Race race) {
            var rs = Ctx.RaceSeries.FirstOrDefault(rs => rs.Id == race.RaceSeriesId);
            if (rs != null) {
                race.AmountOfOpponents = rs.ParticipantCount - 1;
            }
            race.CurrentTyre = race.SelectedTyres[0];

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

                ViewBag.TrackWeatherList = Enum.GetValues(typeof(Weather))
                            .Cast<Weather>()
                            .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                            .ToList();

                ViewBag.TrackStateList = Enum.GetValues(typeof(TrackState))
                    .Cast<TrackState>()
                    .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
                    .ToList();

                ViewBag.TyreList = race.SelectedTyres
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

            ViewBag.TrackWeatherList = Enum.GetValues(typeof(Weather))
                .Cast<Weather>()
                .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                .ToList();

            ViewBag.TrackStateList = Enum.GetValues(typeof(TrackState))
                .Cast<TrackState>()
                .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
            .ToList();

            ViewBag.TyreList = race.SelectedTyres
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
        public IActionResult SetWeather(int id, Weather trackWeather) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.TrackWeather = trackWeather;
            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", race);
        }

        [HttpPost]
        public IActionResult SetState(int id, TrackState trackState) {
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

        [HttpPost]
        public IActionResult Pit(int id, TyreCompound newTyre, bool refueling) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.CurrentTyre = newTyre;
            race.NumberOfStops++;
            if (refueling) {
                race.LastRefuelLap = race.LapCount;
            }

            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", race);
        }

        [HttpPost]
        public IActionResult RemoveOpponent(int id) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.AmountOfOpponents--;
            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", race);
        }

        private int FindAndSaveProgress(Race race) {
            var raceProgress = Ctx.RaceProgresses.FirstOrDefault(rp => rp.RaceId == race.Id);
            if (raceProgress == null) return -1;
            raceProgress.RaceSnapshots.Add(race);
            Ctx.Entry(race).State = EntityState.Modified;
            Ctx.SaveChanges();
            return 0;
        }
    }
}
