using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClearCity.DAL;
using ClearCity.Models;

namespace ClearCity.Controllers
{
    public class MicrodistrictController : Controller
    {
        private ClearCityContext db = new ClearCityContext();

        // GET: Microdistrict
        public ActionResult Index(int? district)
        {
            var microdistricts = db.Microdistricts.Include(m => m.District);
            if (district != null && district != 0)
            {
                microdistricts = microdistricts.Where(p => p.DistrictId == district);
            }

            List<District> districts = db.Districts.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            districts.Insert(0, new District { DistrictName = "Все", DistrictId = 0 });           

            MicrodistrictListViewModel plvm = new MicrodistrictListViewModel
            {
                Microdistricts = microdistricts.ToList(),
                District = new SelectList(districts, "DistrictId", "DistrictName"),

            };
            return View(plvm);
        }

        // GET: Microdistrict/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Microdistrict microdistrict = db.Microdistricts.Find(id);
            if (microdistrict == null)
            {
                return HttpNotFound();
            }
            return View(microdistrict);
        }

        // GET: Microdistrict/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName");
            return View();
        }

        // POST: Microdistrict/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MicrodistrictId,MicrodistrictName,DistrictId")] Microdistrict microdistrict)
        {
            if (ModelState.IsValid)
            {
                db.Microdistricts.Add(microdistrict);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName", microdistrict.DistrictId);
            return View(microdistrict);
        }

        // GET: Microdistrict/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Microdistrict microdistrict = db.Microdistricts.Find(id);
            if (microdistrict == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName", microdistrict.DistrictId);
            return View(microdistrict);
        }

        // POST: Microdistrict/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MicrodistrictId,MicrodistrictName,DistrictId")] Microdistrict microdistrict)
        {
            if (ModelState.IsValid)
            {
                db.Entry(microdistrict).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName", microdistrict.DistrictId);
            return View(microdistrict);
        }

        // GET: Microdistrict/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Microdistrict microdistrict = db.Microdistricts.Find(id);
            if (microdistrict == null)
            {
                return HttpNotFound();
            }
            return View(microdistrict);
        }

        // POST: Microdistrict/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Microdistrict microdistrict = db.Microdistricts.Find(id);
            db.Microdistricts.Remove(microdistrict);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AmountRate()
        {
            var micro = from m in db.Microdistricts
                        join h in db.Houses
                        on m.MicrodistrictId equals h.MicrodistrictId
                        group h by h.MicrodistrictId into g
                        select new MicrodistrictStatistic
                        {
                            MicrodistrictName = (from m in db.Microdistricts
                                              join gr in g
                                              on m.MicrodistrictId equals g.Key
                                              select m.MicrodistrictName).FirstOrDefault(),
                            AmountOfCans = g.Select(h => h.AmountOfCans).Sum()
                        };

            return View(micro.ToList());
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
