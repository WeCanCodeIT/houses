using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scoreboard.Models;

namespace Scoreboard.Controllers
{
    public class TeamsController : Controller
    {
        private TeamScoreboardEntities db = new TeamScoreboardEntities();

        // GET: Teams
        public ActionResult Index()
        {
            return View(db.Teams.ToList());
        }
        //GET: MY TEST
        public ActionResult ViewScore()
        {
            //Hopper is Hopper
            var hopper = 0;
            var getHopper = from entry in db.PointSubmissions
                            where entry.TeamID == 1
                            select entry;
            foreach(var score in getHopper)
            {
                hopper += score.Points;
            }
            ViewBag.hopperScore = hopper;
            
            
            //Lovelace is Easley
            var lovelace = 0;
            var getLovelace = from entry in db.PointSubmissions
                            where entry.TeamID == 2
                            select entry;
            foreach (var score in getLovelace)
            {
                lovelace += score.Points;
            }
            ViewBag.lovelaceScore = lovelace;

            //Gates is Lawson
            var gates = 0;
            var getGates = from entry in db.PointSubmissions
                            where entry.TeamID == 3
                            select entry;
            foreach (var score in getGates)
            {
                gates += score.Points;
            }
            ViewBag.gatesScore = gates;

            //Jobs is Turing
            var jobs = 0;
            var getJobs = from entry in db.PointSubmissions
                            where entry.TeamID == 4
                            select entry;
            foreach (var score in getJobs)
            {
                jobs += score.Points;
            }
            ViewBag.jobsScore = jobs;


            //Return an empty view with the Team Model
            return View();

        }
        
        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamID,TeamName,TeamPoints,TeamDescription,TeamImage")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamID,TeamName,TeamPoints,TeamDescription,TeamImage")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
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
