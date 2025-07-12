using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaceStrategyApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using RaceStrategyApp.ODataClient;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.OData.Client;

namespace RaceStrategyApp.Controllers {
    public class RaceController : BaseController {

        public async virtual Task<IActionResult> All() {
            var races = await Cont.Race.ExecuteAsync();
            List<Models.Race> retval = new List<Models.Race>();

            foreach (var race in races) {
                retval.Add(race);
            }

            return View(retval);
        }

        [HttpPost]
        public async virtual Task<IActionResult> All(int? RSid) {
            var races = await Cont.Race.ExecuteAsync();
            List<Models.Race > retval = new List<Models.Race>();

            foreach (var race in races) {if (race.RaceSeriesId == RSid) retval.Add(race);
            }

            return View(retval);
        }

        public async Task<IActionResult> NewRace() {
            var race = new Models.Race() {
                TrackState = Models.TrackState.green,
                Damage = false,
                TerminalDamage = false,
                LapCount = 0,
                NumberOfStops = 0,
                LastRefuelLap = 0,
                AmountOfOpponents = 0,
            };
            race.SelectedTyres.Add(Models.TyreCompound.generic);

            var series = await Cont.RaceSeries.ExecuteAsync();
            var raceSeriesList = series
                .Select(rs => new SelectListItem { Value = rs.Id.ToString(), Text = rs.Name })
                .ToList();
            ViewBag.RaceSeriesList = raceSeriesList;

            ViewBag.TrackWeatherList = Enum.GetValues(typeof(Models.Weather))
                .Cast<Models.Weather>()
                .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                .ToList();

            return View(race);
        }

        [HttpPost]
        public async Task<IActionResult> NewRace(Models.Race race) {
            var raceSeries = await Cont.RaceSeries.ExecuteAsync();
            var rs = raceSeries.FirstOrDefault(rs => rs.Id == race.RaceSeriesId);
            if (rs != null) {
                race.AmountOfOpponents = rs.ParticipantCount - 1;
            }
            race.CurrentTyre = race.SelectedTyres[0];

            if (ModelState.IsValid) {
                Cont.AddToRace(race);
                await Cont.SaveChangesAsync();

                return RedirectToAction("Race", "Race", new { id = race.Id });
            }
            return View(race);
        }

        public async Task<IActionResult> Race(int? id) {
            ViewData["RaceStarted"] = false;

            if (id == null) return NotFound();
            var races = await Cont.Race.ExecuteAsync();
            var race = races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            if (Ctx.RaceProgresses.Any(rp => rp.RaceId == id) == true) {
                ViewData["RaceStarted"] = true;

                if (race.LapCount + 1 <= race.NumberOfLaps) {
                    ViewData["LapCount++?"] = true;
                }
                else ViewData["LapCount++?"] = false;

                ViewBag.TrackWeatherList = Enum.GetValues(typeof(Models.Weather))
                            .Cast<Models.Weather>()
                            .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                            .ToList();

                ViewBag.TrackStateList = Enum.GetValues(typeof(Models.TrackState))
                    .Cast<Models.TrackState>()
                    .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
                    .ToList();

                ViewBag.TyreList = race.SelectedTyres
                    .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
                    .ToList();
            }

            return View(race);
        }

        [HttpPost]
        public async Task<IActionResult> Race(int id) {
            ViewData["RaceStarted"] = true;
            var races = await Cont.Race.ExecuteAsync();
            var race = races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            ViewBag.TrackWeatherList = Enum.GetValues(typeof(Models.Weather))
                .Cast<Models.Weather>()
                .Select(w => new SelectListItem { Text = w.ToString(), Value = w.ToString() })
                .ToList();

            ViewBag.TrackStateList = Enum.GetValues(typeof(Models.TrackState))
                .Cast<Models.TrackState>()
                .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
            .ToList();

            ViewBag.TyreList = race.SelectedTyres
                    .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
                    .ToList();


            var nr = await Cont.RaceProgress.ExecuteAsync();
            var newR = nr.FirstOrDefault(nr => nr.RaceId == id);
            if (newR == null) {
                Models.RaceProgress newRace = new Models.RaceProgress() {
                    RaceId = id,
                    RaceSnapshot = race
                };
                Cont.AddToRaceProgress(newRace);
                await Cont.SaveChangesAsync();
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

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult SetWeather(int id, Models.Weather trackWeather) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.TrackWeather = trackWeather;
            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult SetState(int id, Models.TrackState trackState) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.TrackState = trackState;

            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
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

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult Pit(int id, Models.TyreCompound newTyre, bool refueling) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.CurrentTyre = newTyre;
            race.NumberOfStops++;
            if (refueling) {
                race.LastRefuelLap = race.LapCount;
            }

            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult RemoveOpponent(int id) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.AmountOfOpponents--;
            int res = FindAndSaveProgress(race);
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        private int FindAndSaveProgress(Models.Race race) {
            var raceProgress = Ctx.RaceProgresses.FirstOrDefault(rp => rp.RaceId == race.Id);
            if (raceProgress == null) return -1;
            var newRace = new Models.RaceProgress() {
                RaceId = race.Id,
                RaceSnapshot = race
            };
            Ctx.RaceProgresses.Add(newRace);
            Ctx.SaveChanges();
            return 0;
        }

        //private Models.Race RetypeRace(ODataClient.Race r) {
        //    Models.Race race = new Models.Race {
        //        Id = r.Id,
        //        AmountOfOpponents = r.AmountOfOpponents,
        //        CurrentTyre = r.CurrentTyre,
        //        Damage = r.Damage,
        //        LapCount = r.LapCount,
        //        TerminalDamage = r.TerminalDamage,
        //        LastRefuelLap = r.LastRefuelLap,
        //        MandatoryStops = r.MandatoryStops,
        //        Name = r.Name,
        //        RaceSeriesId = r.RaceSeriesId,
        //        NumberOfLaps = r.NumberOfLaps,
        //        NumberOfStops = r.NumberOfStops,
        //        Position = r.Position,
        //        Refueling = r.Refueling,
        //        TrackState = r.TrackState,
        //        TrackWeather = r.TrackWeather,
        //        SelectedTyres = new List<Models.TyreCompound>(r.SelectedTyres)
        //    };
        //    return race;
        //}
    }
}
