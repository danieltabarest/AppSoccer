using Backend.Helpers;
using Backend.Models;
using Domain;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using System.Web.UI;

namespace Backend.Controllers
{
    [Authorize]

    public class TournamentsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();
        FilesHelper fileshelper = new FilesHelper();


        // GET: Matches/Edit/5
        public async Task<ActionResult> EditMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = await db.Matches.FindAsync(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            ViewBag.DateId = new SelectList(db.Dates, "DateId", "Name", match.DateId);
            ViewBag.LocalId = new SelectList(db.Teams, "TeamId", "Name", match.LocalId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", match.StatusId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", match.TournamentGroupId);
            ViewBag.VisitorId = new SelectList(db.Teams, "TeamId", "Name", match.VisitorId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMatch(Match match)
        {
            if (ModelState.IsValid)
            {
                db.Entry(match).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("DetailsDate/{0}", match.DateId));
            }
            ViewBag.DateId = new SelectList(db.Dates, "DateId", "Name", match.DateId);
            ViewBag.LocalId = new SelectList(db.Teams, "TeamId", "Name", match.LocalId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", match.StatusId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", match.TournamentGroupId);
            ViewBag.VisitorId = new SelectList(db.Teams, "TeamId", "Name", match.VisitorId);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<ActionResult> DeleteMatch(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var match = await db.Matches.FindAsync(id);

                if (match == null)
                {
                    return HttpNotFound();
                }

                db.Matches.Remove(match);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("DetailsDate/{0}", match.DateId));
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return RedirectToAction(string.Format("DetailsDate/{0}", 1));
            }
        }



        // GET: Matches/Create
        public async Task<ActionResult> CreateMatch(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var dates = await db.Dates.FindAsync(id);
                if (dates == null)
                {
                    return HttpNotFound();
                }

                ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
                ViewBag.LocalId = new SelectList(db.Teams.Where(y => y.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");
                ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
                ViewBag.VisitorId = new SelectList(db.Teams.Where(y => y.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");
                ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == dates.TournamentId).OrderBy(x => x.Name), "TournamentGroupId", "Name");
                var view = new MatchView { DateId = dates.DateId };
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMatch(MatchView view)
        {
            try
            {
                if (view.DateString == null)
                {
                    ShowMessage();
                    return RedirectToAction(string.Format("DetailsDate/{0}", view.DateId));
                }
                if (ModelState.IsValid)
                {
                    view.StatusId = 1;
                    view.DateTime = Convert.ToDateTime(string.Format("{0} {1}", view.DateString, view.TimeString));
                    var match = ToMatch(view);
                    db.Matches.Add(match);
                    await db.SaveChangesAsync();
                    return RedirectToAction(string.Format("DetailsDate/{0}", match.DateId));
                }

                ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LocalLeagueId);
                ViewBag.LocalId = new SelectList(db.Teams.Where(y => y.LeagueId == view.LocalLeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.LocalId);
                ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.VisitorLeagueId);
                ViewBag.VisitorId = new SelectList(db.Teams.Where(y => y.LeagueId == view.VisitorLeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.VisitorId);
                ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentGroupId == view.TournamentGroupId).OrderBy(x => x.Name), "TournamentGroupId", "Name");

                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return RedirectToAction(string.Format("DetailsDate/{0}", view.DateId));
                throw;
            }
        }

        private void ShowMessage()
        {
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Func()", true);
            //Page.ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('Error date invalid');", true);
        }

        private Match ToMatch(MatchView view)
        {
            return new Match
            {
                Date = view.Date,
                DateId = view.DateId,
                DateTime = view.DateTime,
                LocalId = view.LocalId,
                Predictions = view.Predictions,
                StatusId = view.StatusId,
                TournamentGroupId = view.TournamentGroupId,
                VisitorId = view.VisitorId,
            };
        }


        public async Task<ActionResult> DeleteDate(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var date = await db.Dates.FindAsync(id);

                if (date == null)
                {
                    return HttpNotFound();
                }

                db.Dates.Remove(date);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }
        public async Task<ActionResult> EditDate(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var date = await db.Dates.FindAsync(id);

                if (date == null)
                {
                    return HttpNotFound();
                }

                return View(date);
            }
            catch (Exception ex)
            {

                fileshelper.ErrorLogging(ex);
                return View();

            }
        }


        // GET: TournamentGroups/Details/5
        public async Task<ActionResult> DetailsGroup(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var tournamentGroup = await db.TournamentGroups.FindAsync(id);
                if (tournamentGroup == null)
                {
                    return HttpNotFound();
                }
                return View(tournamentGroup);
            }
            catch (Exception ex)
            {

                fileshelper.ErrorLogging(ex);
                return View();

            }
        }



        public async Task<ActionResult> DetailsDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dates = await db.Dates.FindAsync(id);
            if (dates == null)
            {
                return HttpNotFound();
            }
            return View(dates);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDate(Date date)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(date).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
                }

                return View(date);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }
        public async Task<ActionResult> CreateDate(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var tournament = await db.Tournaments.FindAsync(id);

                if (tournament == null)
                {
                    return HttpNotFound();
                }

                var view = new Date { TournamentId = tournament.TournamentId, };
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Dates.Add(date);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
            }

            return View(date);
        }
        public async Task<ActionResult> DeleteGroup(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var tournamentGroup = await db.TournamentGroups.FindAsync(id);

                if (tournamentGroup == null)
                {
                    return HttpNotFound();
                }

                db.TournamentGroups.Remove(tournamentGroup);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
            }
            catch (Exception ex)
            {

                fileshelper.ErrorLogging(ex);
                return View();
            }

        }
        public async Task<ActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroup);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
            }

            return View(tournamentGroup);
        }
        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.ToListAsync());
        }

        // GET: Tournaments/Details/5
        public async Task<ActionResult> CreateGroup(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var tournament = await db.Tournaments.FindAsync(id);
                if (tournament == null)
                {
                    return HttpNotFound();
                }

                var view = new TournamentGroup { TournamentId = tournament.TournamentId, };
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.TournamentGroups.Add(tournamentGroup);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
            }

            //ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name", tournamentGroup.TournamentId);
            return View(tournamentGroup);
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }



        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TournamentView view)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var pic = string.Empty;
                    var folder = "~/Content/Logos";

                    if (view.LogoFile != null)
                    {
                        pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }

                    var tournament = ToTournament(view);
                    tournament.Logo = pic;
                    db.Tournaments.Add(tournament);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();

            }
        }
        private Tournament ToTournament(Tournament view)
        {
            return new Tournament
            {
                TournamentId = view.TournamentId,
                IsActive = view.IsActive,
                Dates = view.Dates,
                Logo = view.Logo,
                Name = view.Name,
                Order = view.Order,
                Groups = view.Groups

            };
        }
        private TournamentView ToView(Tournament tournament)
        {
            return new TournamentView
            {
                TournamentId = tournament.TournamentId,
                IsActive = tournament.IsActive,
                Dates = tournament.Dates,
                Logo = tournament.Logo,
                Name = tournament.Name,
                Order = tournament.Order,
                Groups = tournament.Groups
            };

        }


        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = ToView(tournament);
            return View(view);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TournamentView view)
        {

            if (ModelState.IsValid)
            {

                var pic = view.Logo;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var league = ToTournament(view);
                league.Logo = pic;
                db.Entry(league).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tournament tournament = await db.Tournaments.FindAsync(id);
            db.Tournaments.Remove(tournament);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ///-----
        /// // GET: TournamentTeams/Create
        public async Task<ActionResult> CreateTeam(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var tournamentgroups = await db.TournamentGroups.FindAsync(id);
                if (tournamentgroups == null)
                {
                    return HttpNotFound();
                }


                ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
                ViewBag.TeamId = new SelectList(db.Teams.Where(y => y.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");
                var view = new TournamentTeamView { TournamentGroupId = tournamentgroups.TournamentGroupId };
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        // POST: TournamentTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TournamentTeamView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tournamentTeam = ToTournamentTeam(view);
                    db.TournamentTeams.Add(tournamentTeam);
                    await db.SaveChangesAsync();
                    return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));
                }

                ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.Team.LeagueId);
                ViewBag.TeamId = new SelectList(db.Teams.Where(y => y.LeagueId == view.Team.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.Team.TeamId);
                return View(view);
            }
            catch (Exception ex)
            {

                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        // GET: TournamentTeams/Edit/5
        public async Task<ActionResult> EditTeam(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TournamentTeam tournamentTeam = await db.TournamentTeams.FindAsync(id);
                if (tournamentTeam == null)
                {
                    return HttpNotFound();
                }

                ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", tournamentTeam.Team.LeagueId);
                ViewBag.TeamId = new SelectList(db.Teams.Where(y => y.LeagueId == tournamentTeam.Team.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", tournamentTeam.Team.TeamId);
                var view = ToView(tournamentTeam);
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        private TournamentTeamView ToView(TournamentTeam tournamentTeam)
        {
            return new TournamentTeamView
            {
                AgainstGoals = tournamentTeam.AgainstGoals,
                FavorGoals = tournamentTeam.FavorGoals,
                LeagueId = tournamentTeam.Team.LeagueId,
                MatchesLost = tournamentTeam.MatchesLost,
                MatchesPlayed = tournamentTeam.MatchesPlayed,
                MatchesTied = tournamentTeam.MatchesTied,
                MatchesWon = tournamentTeam.MatchesWon,
                Points = tournamentTeam.Points,
                Position = tournamentTeam.Position,
                Team = tournamentTeam.Team,
                TeamId = tournamentTeam.TeamId,
                TournamentGroup = tournamentTeam.TournamentGroup,
                TournamentGroupId = tournamentTeam.TournamentGroupId,
                TournamentTeamId = tournamentTeam.TournamentTeamId

            };
        }

        // POST: TournamentTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = ToTournamentTeam(view);
                db.Entry(tournamentTeam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));
            }
            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.Team.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(y => y.LeagueId == view.Team.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.Team.TeamId);
            return View(view);
        }


        private TournamentTeam ToTournamentTeam(TournamentTeamView view)
        {
            return new TournamentTeam
            {
                AgainstGoals = view.AgainstGoals,
                FavorGoals = view.FavorGoals,
                MatchesLost = view.MatchesLost,
                MatchesPlayed = view.MatchesPlayed,
                MatchesTied = view.MatchesTied,
                MatchesWon = view.MatchesWon,
                Points = view.Points,
                Position = view.Position,
                Team = view.Team,
                TeamId = view.TeamId,
                TournamentGroup = view.TournamentGroup,
                TournamentGroupId = view.TournamentGroupId,
                TournamentTeamId = view.TournamentTeamId

            };
        }
        // GET: TournamentTeams/Delete/5
        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentteams = await db.TournamentTeams.FindAsync(id);

            if (tournamentteams == null)
            {
                return HttpNotFound();
            }

            db.TournamentTeams.Remove(tournamentteams);
            await db.SaveChangesAsync();
            return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentteams.TournamentGroupId));
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
