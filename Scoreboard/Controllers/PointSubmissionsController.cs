using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scoreboard.Models;
using Newtonsoft.Json;

namespace Scoreboard.Controllers
{
    public class PointSubmissionsController : Controller
    {
        private TeamScoreboardEntities db = new TeamScoreboardEntities();

        // GET: PointSubmissions
        public ActionResult Index()
        {
            var pointSubmissions = db.PointSubmissions.Include(p => p.Team);
            return View(pointSubmissions.ToList());
        }

// GET: PointSubmissions/Details/5
public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointSubmission pointSubmission = db.PointSubmissions.Find(id);
            if (pointSubmission == null)
            {
                return HttpNotFound();
            }
            return View(pointSubmission);
        }

        //GET: View Points Submitted for each house
        public ActionResult HopperSubmissions()
        {
            var hoppers = from each in db.PointSubmissions
                          where each.TeamID == 1
                          select each;
            return View(hoppers.ToList());
        }

        public ActionResult LovelaceSubmissions()
        {
            var lovelaces = from each in db.PointSubmissions
                          where each.TeamID == 2
                          select each;
            return View(lovelaces.ToList());
        }

        public ActionResult GatesSubmissions()
        {
            var gateses = from each in db.PointSubmissions
                          where each.TeamID == 3
                          select each;
            return View(gateses.ToList());
        }

        public ActionResult JobsSubmissions()
        {
            var jobses = from each in db.PointSubmissions
                          where each.TeamID == 4
                          select each;
            return View(jobses.ToList());
        }

        // GET: PointSubmissions/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName");
            return View();
        }


        // POST: PointSubmissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubmissionID,TeamID,Points,Description")] PointSubmission pointSubmission)
        {
            if (ModelState.IsValid)
            {
                db.PointSubmissions.Add(pointSubmission);
                db.SaveChanges();

      

                //SLACK FOR POSTING HOUSE POINTS SUBMISSIONS TO GENERAL BOOTCAMP CHANNEL
                var vm = new { channel = "#test", username = "HousePoints", text = String.Format("{0} POINTS for {1} House!", pointSubmission.Points, pointSubmission.Team.TeamName), icon_emoji = ":trophy:" };
                using (var client = new WebClient())
                {
                    var dataString = JsonConvert.SerializeObject(vm);
                    var url = "https://hooks.slack.com/services/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";
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    string result = client.UploadString(new Uri(url), "POST", dataString);
                }

                ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", pointSubmission.TeamID);


                return RedirectToAction("Index");
            }

            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", pointSubmission.TeamID);

            return View(pointSubmission);
        }

        // GET: PointSubmissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointSubmission pointSubmission = db.PointSubmissions.Find(id);
            if (pointSubmission == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", pointSubmission.TeamID);
            return View(pointSubmission);
        }

        // POST: PointSubmissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubmissionID,TeamID,Points,Description")] PointSubmission pointSubmission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pointSubmission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", pointSubmission.TeamID);
            return View(pointSubmission);
        }

        // GET: PointSubmissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointSubmission pointSubmission = db.PointSubmissions.Find(id);
            if (pointSubmission == null)
            {
                return HttpNotFound();
            }
            return View(pointSubmission);
        }

        // POST: PointSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PointSubmission pointSubmission = db.PointSubmissions.Find(id);
            db.PointSubmissions.Remove(pointSubmission);
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
