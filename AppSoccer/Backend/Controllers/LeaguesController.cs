using Backend.Helpers;
using Backend.Models;
using Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace Backend.Controllers
{
    [Authorize]
    public class LeaguesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();
         FilesHelper fileshelper = new FilesHelper();
        public async Task<ActionResult> DeleteTeam(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var team = await db.Teams.FindAsync(id);

                if (team == null)
                {
                    return HttpNotFound();
                }

                db.Teams.Remove(team);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", team.LeagueId));
            } 
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }
        public async Task<ActionResult> EditTeam(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var team = await db.Teams.FindAsync(id);

                if (team == null)
                {
                    return HttpNotFound();
                }

                var view = ToView(team);
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
        public async Task<ActionResult> EditTeam(TeamView view)
        {
            try
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

                    var team = ToTeam(view);
                    team.Logo = pic;
                    db.Entry(team).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction(string.Format("Details/{0}", view.LeagueId));
                }

                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
                
            }
        }
        public async Task<ActionResult> CreateTeam(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var league = await db.Leagues.FindAsync(id);

                if (league == null)
                {
                    return HttpNotFound();
                }

                var view = new TeamView { LeagueId = league.LeagueId, };
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
        public async Task<ActionResult> CreateTeam(TeamView view)
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

                    var team = ToTeam(view);
                    team.Logo = pic;
                    db.Teams.Add(team);
                    await db.SaveChangesAsync();
                    return RedirectToAction(string.Format("Details/{0}", view.LeagueId));
                }

                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        private Team ToTeam(TeamView view)
        {
            return new Team
            {
                Initials = view.Initials,
                League = view.League,
                LeagueId = view.LeagueId,
                Logo = view.Logo,
                Name = view.Name,
                TeamId = view.TeamId,

            };
        }
        private TeamView ToView(Team team)
        {
            return new TeamView
            {
                Initials = team.Initials,
                League = team.League,
                LeagueId = team.LeagueId,
                Logo = team.Logo,
                Name = team.Name,
                TeamId = team.TeamId,

            };

        }
        public async Task<ActionResult> Index()
        {
            return View(await db.Leagues.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueView view)
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

                    var league = ToLeague(view);
                    league.Logo = pic;
                    db.Leagues.Add(league);
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

        private League ToLeague(LeagueView view)
        {
            return new League
            {
                LeagueId = view.LeagueId,
                Logo = view.Logo,
                Name = view.Name,
                Teams = view.Teams,
            };
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var league = await db.Leagues.FindAsync(id);

                if (league == null)
                {
                    return HttpNotFound();
                }

                var view = ToView(league);
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        private LeagueView ToView(League league)
        {
            return new LeagueView
            {
                LeagueId = league.LeagueId,
                Logo = league.Logo,
                Name = league.Name,
                Teams = league.Teams,
            };
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeagueView view)
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

                var league = ToLeague(view);
                league.Logo = pic;
                db.Entry(league).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                League league = await db.Leagues.FindAsync(id);
                if (league == null)
                {
                    return HttpNotFound();
                }

                return View(league);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            db.Leagues.Remove(league);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
