using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backend.Controllers
{
    public class GenericController : Controller
    {
        private DataContextLocal db = new DataContextLocal();
        public JsonResult GetTeams(int LeagueId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var teams = db.Teams.Where(m => m.LeagueId == LeagueId);
            return Json(teams);
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