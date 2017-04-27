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
using Backend.Helpers;

namespace Backend.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private DataContextLocal db = new DataContextLocal();
        FilesHelper fileshelper = new FilesHelper();
        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.FavoriteTeam).Include(u => u.UserType);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
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

                    var user = ToUser(view);
                    user.Picture = pic;
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", view.FavoriteTeamId);
                ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", view.UserTypeId);
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", user.FavoriteTeamId);
                ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", user.UserTypeId);
                var view = ToView(user);
                return View(view);
            }
            catch (Exception ex)
            {
                fileshelper.ErrorLogging(ex);
                return View();
            }
        }

        private UserView ToView(User user)
        {
            return new UserView
            {
                Email = user.Email,
                FavoriteTeam = user.FavoriteTeam,
                FavoriteTeamId = user.FavoriteTeamId,
                FirstName = user.FirstName,
                GroupUsers = user.GroupUsers,
                //LogoFile = user.LogoFile,
                LastName = user.LastName,
                Points = user.Points,
                UserGroups = user.UserGroups,
                NickName = user.NickName,
                Picture = user.Picture,
                Predictions = user.Predictions,
                UserType = user.UserType,
                UserTypeId = user.UserTypeId,
                UserId = user.UserId,
            };
        }

     


        private User ToUser(UserView view)
        {
            return new User
            {
                Email = view.Email,
                FavoriteTeam = view.FavoriteTeam,
                FavoriteTeamId = view.FavoriteTeamId,
                FirstName = view.FirstName,
                LastName = view.LastName,
                GroupUsers = view.GroupUsers,
                //LogoFile = view.LogoFile,
                NickName = view.NickName,
                Picture = view.Picture,
                Predictions = view.Predictions,
                UserType = view.UserType,
                Points = view.Points,
                UserGroups = view.UserGroups,
                UserTypeId = view.UserTypeId,
                UserId = view.UserId,

            };
        }
        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Picture;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var user = ToUser(view);
                user.Picture = pic;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", view.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", view.UserTypeId);
            return View(view);

            //if (ModelState.IsValid)
            //{
            //    db.Entry(user).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", user.FavoriteTeamId);
            //ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", user.UserTypeId);
            //return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
