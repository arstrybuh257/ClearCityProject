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
    public class HouseController : Controller
    {
        private ClearCityContext db = new ClearCityContext();

        // GET: House
        public ActionResult Index(string sortOrder, int? district, int? microdistrict, string searchString)
        {
            ViewBag.AmountSortParm = String.IsNullOrEmpty(sortOrder) ? "amount" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            var houses = from h in db.Houses
                         select h;
            if (!String.IsNullOrEmpty(searchString))
            {
                houses = houses.Where(s => s.Adress.Contains(searchString));
            }
            if (district != null && district != 0)
            {
                houses = houses.Where(p => p.Microdistrict.District.DistrictId == district);
            }
            if (microdistrict != null && microdistrict != 0)
            {
                houses = houses.Where(p => p.Microdistrict.MicrodistrictId == microdistrict);
            }

            switch (sortOrder)
            {
                case "amount":
                    houses = houses.OrderBy(h => h.AmountOfCans);
                    break;
                case "name":
                    houses = houses.OrderBy(h => h.Microdistrict.District.DistrictName).
                        ThenBy(h => h.Microdistrict.MicrodistrictName);
                    break;
                default:
                    houses = houses.OrderByDescending(h => h.AmountOfCans);
                    break;
            }

            List<District> districts = db.Districts.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            districts.Insert(0, new District { DistrictName = "Все", DistrictId = 0 });
            List<Microdistrict> microdistricts = db.Microdistricts.ToList();
            microdistricts.Insert(0, new Microdistrict { MicrodistrictName = "Все", MicrodistrictId = 0 });

            HouseListViewModel plvm = new HouseListViewModel
            {
                Houses = houses.ToList(),
                District = new SelectList(districts, "DistrictId", "DistrictName"),
                Microdistrict = new SelectList(microdistricts, "MicrodistrictId", "MicrodistrictName")
            };
            return View(plvm);
        }

        // GET: House/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // GET: House/Create
        public ActionResult Create()
        {
            ViewBag.MicrodistrictId = new SelectList(db.Microdistricts, "MicrodistrictId", "MicrodistrictName");
            return View();
        }

        // POST: House/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HouseId,Adress,AmountOfCans,MicrodistrictId")] House house)
        {
            if (ModelState.IsValid)
            {
                db.Houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MicrodistrictId = new SelectList(db.Microdistricts, "MicrodistrictId", "MicrodistrictName", house.MicrodistrictId);
            return View(house);
        }

        // GET: House/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            ViewBag.MicrodistrictId = new SelectList(db.Microdistricts, "MicrodistrictId", "MicrodistrictName", house.MicrodistrictId);
            return View(house);
        }

        // POST: House/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HouseId,Adress,AmountOfCans,MicrodistrictId")] House house)
        {
            if (ModelState.IsValid)
            {
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MicrodistrictId = new SelectList(db.Microdistricts, "MicrodistrictId", "MicrodistrictName", house.MicrodistrictId);
            return View(house);
        }

        // GET: House/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: House/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            House house = db.Houses.Find(id);
            db.Houses.Remove(house);
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
