using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryProject.DAL;
using InventoryProject.Models;
using PagedList;
using InventoryProject.ViewModels;

namespace InventoryProject.Controllers
{
    public class ItemsController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Items
        public ActionResult Index(string sortOrder, string searchString, string searchCategory)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.QuantitySortParam = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            var items = from i in db.Items
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchCategory)
                {
                    case "name":
                        items = items.Where(i => i.Name.Contains(searchString));
                        break;
                    case "description":
                        items = items.Where(i => i.Description.Contains(searchString));
                        break;
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name);
                    break;
                case "quantity":
                    items = items.OrderBy(i => i.Quantity);
                    break;
                case "quantity_desc":
                    items = items.OrderByDescending(i => i.Quantity);
                    break;
                default: items = items.OrderBy(i => i.Name);
                    break;
            }
            return View(items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult DetailsReadOnly(int id)
        {
            Item item = db.Items.Find(id);
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Quantity,Description,ImageURL")] Item item)
        {

            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Quantity,Description,ImageURL")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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

        public ActionResult StoreFront()
        {
            var model = new ItemCustomer();
            model.Items = db.Items;
            model.Customers = db.Customers;
            Customer customer = new Customer();
            customer.FirstName = "Anonymous";
            model.currentCustomer = customer;
            return View(model);
        }

        public ActionResult ItemReviewCountReport()
        {
            List<Item> itemList = new List<Item>();
            foreach(var item in db.Items)
            {
                itemList.Add(item);
            }

            foreach(var item in itemList)
            {
                var reviewList = (from r in db.Reviews
                              where r.ItemId == item.Id
                              select r).ToList();
            }

            var items = from i in itemList
                        orderby i.Reviews.Count() descending
                        select i;

            return View(items);
        }
    }
}
