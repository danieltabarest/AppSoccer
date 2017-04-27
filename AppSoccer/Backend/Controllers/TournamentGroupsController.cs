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
    [Authorize]
    public class TournamentGroupsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: TournamentGroups
        public async Task<ActionResult> Index()
        {
            var tournamentGroups = db.TournamentGroups.Include(t => t.Tournament);
            return View(await tournamentGroups.ToListAsync());
        }

        // GET: TournamentGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            return View(tournamentGroup);
        }

        // GET: TournamentGroups/Create
        public ActionResult Create()
        {
            ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name");
            return View();
        }

        // POST: TournamentGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TournamentGroupId,Name,TournamentId")] TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.TournamentGroups.Add(tournamentGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name", tournamentGroup.TournamentId);
            return View(tournamentGroup);
        }

        // GET: TournamentGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name", tournamentGroup.TournamentId);
            return View(tournamentGroup);
        }

        // POST: TournamentGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TournamentGroupId,Name,TournamentId")] TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name", tournamentGroup.TournamentId);
            return View(tournamentGroup);
        }

        // GET: TournamentGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            return View(tournamentGroup);
        }

        // POST: TournamentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            db.TournamentGroups.Remove(tournamentGroup);
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
