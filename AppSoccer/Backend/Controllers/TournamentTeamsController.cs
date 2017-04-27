using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backend.Models;
using Domain;

namespace Backend.Controllers
{
    public class TournamentTeamsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: TournamentTeams
        public async Task<ActionResult> Index()
        {
            var tournamentTeams = db.TournamentTeams.Include(t => t.Team).Include(t => t.TournamentGroup);
            return View(await tournamentTeams.ToListAsync());
        }

        // GET: TournamentTeams/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(tournamentTeam);
        }

        // GET: TournamentTeams/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name");
            return View();
        }

        // POST: TournamentTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TournamentTeamId,TournamentGroupId,TeamId,MatchesPlayed,MatchesWon,MatchesLost,MatchesTied,FavorGoals,AgainstGoals,Points,Position")] TournamentTeam tournamentTeam)
        {
            if (ModelState.IsValid)
            {
                db.TournamentTeams.Add(tournamentTeam);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", tournamentTeam.TeamId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", tournamentTeam.TournamentGroupId);
            return View(tournamentTeam);
        }

        // GET: TournamentTeams/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", tournamentTeam.TeamId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", tournamentTeam.TournamentGroupId);
            return View(tournamentTeam);
        }

        // POST: TournamentTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TournamentTeamId,TournamentGroupId,TeamId,MatchesPlayed,MatchesWon,MatchesLost,MatchesTied,FavorGoals,AgainstGoals,Points,Position")] TournamentTeam tournamentTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentTeam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", tournamentTeam.TeamId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", tournamentTeam.TournamentGroupId);
            return View(tournamentTeam);
        }

        // GET: TournamentTeams/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(tournamentTeam);
        }

        // POST: TournamentTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TournamentTeam tournamentTeam = await db.TournamentTeams.FindAsync(id);
            db.TournamentTeams.Remove(tournamentTeam);
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
