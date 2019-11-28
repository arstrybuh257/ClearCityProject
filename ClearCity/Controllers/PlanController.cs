using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClearCity.Automatization;
using ClearCity.DAL;
using ClearCity.Models;

namespace ClearCity.Controllers
{
    public class PlanController : Controller
    {
        private ClearCityContext db = new ClearCityContext();

        // GET: Plan
        public ActionResult Index()
        {
            var plans = db.Plans.Include(p => p.House).Include(p => p.Team);
            return View(plans.ToList());
        }

        // GET: Plan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // GET: Plan/Create
        public ActionResult Create()
        {
            ViewBag.HouseId = new SelectList(db.Houses, "HouseId", "Adress");
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Plan/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlanId,HouseId,TeamId,Date")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                db.Plans.Add(plan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseId = new SelectList(db.Houses, "HouseId", "Adress", plan.HouseId);
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", plan.TeamId);
            return View(plan);
        }

        // GET: Plan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseId = new SelectList(db.Houses, "HouseId", "Adress", plan.HouseId);
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", plan.TeamId);
            return View(plan);
        }

        // POST: Plan/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanId,HouseId,TeamId,Date")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseId = new SelectList(db.Houses, "HouseId", "Adress", plan.HouseId);
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", plan.TeamId);
            return View(plan);
        }

        // GET: Plan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // POST: Plan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plan plan = db.Plans.Find(id);
            db.Plans.Remove(plan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Report()
        {
            var collection = from p in db.Plans
                            group p by p.TeamId into g
                            select new PlanReport
                            {
                                TeamName = (from t in db.Teams
                                            where t.TeamId == g.Key
                                            select t.TeamName).FirstOrDefault(),
                                AmountOfCans = (from p in g
                                                join h in db.Houses
                                                on p.HouseId equals h.HouseId
                                                select h.AmountOfCans).Sum()

                            };

            return View(collection.ToList());

        }

        public ActionResult Automatization()
        {
            AutoHelper ah = new AutoHelper(db);
            var list = ah.GetPlan(DateTime.Now);

            

            foreach (var p in list)
            {
                db.Plans.Add(p);
            }
            db.SaveChanges();

            return View("Index", db.Plans.ToList());
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
