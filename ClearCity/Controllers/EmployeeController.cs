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
    public class EmployeeController : Controller
    {
        private ClearCityContext db = new ClearCityContext();

        // GET: Employee
        public ActionResult Index(string sortOrder, string searchString, string teamFilter)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var employees = from s in db.Employees
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Name);
                    break;
                case "Date":
                    employees = employees.OrderBy(e => e.BirthDate);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(s => s.BirthDate);
                    break;
                default:
                    employees = employees.OrderBy(e => e.TeamId).ThenBy(e => e.Position);
                    break;
            }
            return View(employees.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Employee/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,Name,BirthDate,Position,Phone,Adress,TeamId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", employee.TeamId);
            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", employee.TeamId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,Name,BirthDate,Position,Phone,Adress,TeamId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", employee.TeamId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ViewResult ShowAgeGroups()
        {
            int less18 = 0;
            int from18To25 = 0;
            int from25To45 = 0;
            int from45To60 = 0;
            int more60 = 0;

            foreach (var e in db.Employees)
            {
                if (DateTime.Now.Year - e.BirthDate.Year < 18)
                {
                    less18++;
                }
                else if (DateTime.Now.Year - e.BirthDate.Year <= 25)
                {
                    from18To25++;
                }
                else if (DateTime.Now.Year - e.BirthDate.Year <= 45)
                {
                    from25To45++;
                }
                else if (DateTime.Now.Year - e.BirthDate.Year <= 60)
                {
                    from45To60++;
                }
                else more60++;
            }

            ViewData["Less18"] = less18;
            ViewData["From18To25"] = from18To25;
            ViewData["From25To45"] = from25To45;
            ViewData["From45To60"] = from45To60;
            ViewData["More60"] = more60;

            return View();
        }

        public ActionResult EmployeeReport()
        {
            var collection = from e in db.Employees
                             group e by e.TeamId into g
                             select new EmployeeReportView
                             {
                                 TeamName = (from e in g
                                             join t in db.Teams
                                             on g.Key equals t.TeamId
                                             where g.Key != 1
                                             select t.TeamName).FirstOrDefault(),
                                 Employees = g.Where(t => t.TeamId != 1).ToList()
                             };

            return View(collection.ToList());
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

        

