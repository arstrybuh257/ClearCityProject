using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClearCity.DAL;
using ClearCity.Models;


namespace ClearCity.Controllers
{
    public class CarController : Controller
    {
        private ClearCityContext db = new ClearCityContext();

        // GET: Car
        public ActionResult Index()
        {
            var cars = db.Cars.Include(c => c.Team);
            return View(cars.ToList());
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            var employee = from t in db.Teams
                           join e in db.Employees on t.TeamId equals e.TeamId
                           join c in db.Cars on t.TeamId equals c.TeamId 
                           where e.Position == "Водитель" && c.CarId == id && id!=1
                           select e.Name;
            var driver = employee.AsEnumerable().ToList();
            if (driver.Count == 0)
            {
                ViewBag.dr = "";
            }
            else ViewBag.dr = driver[0];


            return View(car);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Car/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,Model,Number,DateOfRelease,DateOfLastInspection,TeamId")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", car.TeamId);
            return View(car);
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", car.TeamId);
            return View(car);
        }

        // POST: Car/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarId,Model,Number,DateOfRelease,DateOfLastInspection,TeamId")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", car.TeamId);
            return View(car);
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NeedInspection()
        {
            var cars = db.Cars.Where(c => c.DateOfLastInspection == null ||
            DateTime.Now.Year - c.DateOfLastInspection.Value.Year >= 1).Select(c => c);

            return View(cars.ToList());
        }

        public ActionResult ShowCarAge()
        {
            var cars = db.Cars.Select(c => new CarAge
            {
                Model = c.Model,
                Number = c.Number,
                Age = DateTime.Now.Year - c.DateOfRelease.Value.Year
            });

            return View(cars.ToList());
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
