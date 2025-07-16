using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace RaceStrategyApp.Controllers {
    public class RaceController : BaseController {

        public async virtual Task<IActionResult> All() {;
            try {
                var races = await Cont.Race.ExecuteAsync();
                List<Models.Race> retval = new List<Models.Race>();

                foreach (var race in races) {
                    retval.Add(race);
                }
                return View(retval);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return View();
            }
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
                Damage = false,
                TerminalDamage = false,
                LapCount = 0,
                AmountOfOpponents = 0,
                Name = "",
                PitStop = new() {
                    NumberOfStops = 0,
                    LastRefuelLap = 0,
                },
                TrackInfo = new() {
                    TrackState = Models.TrackState.green
                }
            };

            race.PitStop.RaceId = race.Id;
            race.TrackInfo.RaceId = race.Id;
            race.PitStop.SelectedTyres.Add(Models.TyreCompound.generic);

            var series = await Cont.RaceSeries.ExecuteAsync();
            var raceSeriesList = series
                .Select(rs => new SelectListItem { Value = rs.Id.ToString(), Text = rs.Name })
                .ToList();
            ViewBag.RaceSeriesList = raceSeriesList;

            var weatherList = Enum.GetValues(typeof(Models.Weather))
                .Cast<Models.Weather>()
                .Select(w => new SelectListItem { 
                    Text = Regex.Replace(w.ToString(), "(\\B[A-Z])", " $1"), 
                    Value = w.ToString() 
                })
                .ToList();
            ViewBag.TrackWeatherList = weatherList;

            return View(race);
        }

        [HttpPost]
        public async Task<IActionResult> NewRace(Models.Race race) {
            
            if (race.Name == null) {
                race.Name = "";
            }

            try {
                var raceSeries = await Cont.RaceSeries.ExecuteAsync();
                var rs = raceSeries.FirstOrDefault(rs => rs.Id == race.RaceSeriesId);
                if (rs != null) {
                    race.AmountOfOpponents = rs.ParticipantCount - 1;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }

            race.PitStop.CurrentTyre = race.PitStop.SelectedTyres[0];

            if (ModelState.IsValid) {
                Console.WriteLine(race.PitStop.SelectedTyres[1].ToString());
                //race.PitStop.RaceId = race.Id;
                //race.TrackInfo.RaceId = race.Id;
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


            ViewData["LapCount++?"] = false;
            if (Ctx.RaceProgresses.Any(rp => rp.RaceId == id) == true) {
                ViewData["RaceStarted"] = true;

                if (race.LapCount + 1 <= race.NumberOfLaps) {
                    ViewData["LapCount++?"] = true;
                }

                ViewBag.TrackWeatherList = Enum.GetValues(typeof(Models.Weather))
                            .Cast<Models.Weather>()
                            .Select(w => new SelectListItem {
                                Text = Regex.Replace(w.ToString(), "(\\B[A-Z])", " $1"),
                                Value = w.ToString()
                            })
                            .ToList();

                ViewBag.TrackStateList = Enum.GetValues(typeof(Models.TrackState))
                    .Cast<Models.TrackState>()
                    .Select(t => new SelectListItem { 
                        Text = Regex.Replace(t.ToString(), "(\\B[A-Z])", " $1"), 
                        Value = t.ToString()
                    })
                    .ToList();

                ViewBag.TyreList = race.PitStop.SelectedTyres
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
                            .Select(w => new SelectListItem {
                                Text = Regex.Replace(w.ToString(), "(\\B[A-Z])", " $1"),
                                Value = w.ToString()
                            })
                            .ToList();

            ViewBag.TrackStateList = Enum.GetValues(typeof(Models.TrackState))
                .Cast<Models.TrackState>()
                .Select(t => new SelectListItem {
                    Text = Regex.Replace(t.ToString(), "(\\B[A-Z])", " $1"),
                    Value = t.ToString()
                })
                .ToList();

            ViewBag.TyreList = race.PitStop.SelectedTyres
                    .Select(t => new SelectListItem { Text = t.ToString(), Value = t.ToString() })
                    .ToList();

            var nr = await Cont.RaceProgress.ExecuteAsync();
            var newR = nr.FirstOrDefault(nr => nr.RaceId == id);
            if (newR == null) {
                Models.RaceProgress newRace = new Models.RaceProgress() {
                    Race = race,
                    RaceId = race.Id,
                    RaceSnapshot = new Models.RaceSnapshot() {
                        LapCount = race.LapCount,
                        ChangeName = "first",
                        Change = "first"
                    }
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
                int res = FindAndSaveProgress(race, "Kolo", race.LapCount.ToString());
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

            race.TrackInfo.TrackWeather = trackWeather;
            int res = FindAndSaveProgress(race, "Počasie", Regex.Replace(trackWeather.ToString(), "(\\B[A-Z])", " $1"));
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult SetState(int id, Models.TrackState trackState) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.TrackInfo.TrackState = trackState;

            int res = FindAndSaveProgress(race, "Stav trate", Regex.Replace(trackState.ToString(), "(\\B[A-Z])", " $1"));
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult UpdatePosition(int id, int newPos) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            if (race.Position + newPos >= 1) {
                race.Position += newPos;
                int res = FindAndSaveProgress(race, "Pozícia", race.Position.ToString());
                if (res == -1) return NotFound();
            }

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult Pit(int id, Models.TyreCompound newTyre, bool refueling) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.PitStop.CurrentTyre = newTyre;
            race.PitStop.NumberOfStops++;
            if (refueling) {
                race.PitStop.LastRefuelLap = race.LapCount;
            }

            int res = FindAndSaveProgress(race, "Pit stop", Regex.Replace(newTyre.ToString(), "(\\B[A-Z])", " $1"));
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        [HttpPost]
        public IActionResult RemoveOpponent(int id) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();

            race.AmountOfOpponents--;
            int res = FindAndSaveProgress(race, "Počet Protivníkov", race.AmountOfOpponents.ToString());
            if (res == -1) return NotFound();

            return RedirectToAction("Race", "Race", new { id = race.Id });
        }

        public IActionResult History(int id) {
            var race = Ctx.Races.FirstOrDefault(r => r.Id == id);
            if (race == null) return NotFound();
            var raceProgresses = Ctx.RaceProgresses.Where(rp => rp.RaceId == id).ToList();
            if (raceProgresses == null) return NotFound();

            List<Models.RaceProgress> retval = new();
            foreach (var raceProg in raceProgresses) {
                if (raceProg.RaceSnapshot.ChangeName != "" && raceProg.RaceSnapshot.ChangeName != "Kolo") {
                    retval.Add(raceProg);
                }
            }

            return View(retval);
        }

        private int FindAndSaveProgress(Models.Race race, string changeName, string change) {
            var newRace = new Models.RaceProgress() {
                Race = race,
                RaceId = race.Id,
                RaceSnapshot = new Models.RaceSnapshot() {
                    LapCount = race.LapCount,
                    ChangeName = changeName,
                    Change = change
                }
            };
            Ctx.RaceProgresses.Add(newRace);
            Ctx.SaveChanges();
            return 0;
        }
    }
}
