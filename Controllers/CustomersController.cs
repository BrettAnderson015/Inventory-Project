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
using InventoryProject.ViewModels;

namespace InventoryProject.Controllers
{
    public class CustomersController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Customers
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.LastNameSortParam = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            var customers = from c in db.Customers select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.LastName.Contains(searchString) || c.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "lastName_desc":
                    customers = customers.OrderByDescending(c => c.LastName);
                    break;
                default: customers = customers.OrderBy(i => i.LastName);
                    break;
            }
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CustomerSignIn(string name)
        {
            var cust = db.Customers.Where(c => c.FirstName == name).SingleOrDefault();
            Customer newCustomer = new Customer();
            newCustomer.FirstName = name;
            var model = new ItemCustomer();
            model.Items = db.Items;
            model.Customers = db.Customers;
            model.currentCustomer = newCustomer;


            if (cust == null)
            {
                db.Customers.Add(newCustomer);
                db.SaveChanges();
            }
            else { }
            
            return View("~/Views/Items/StoreFront.cshtml", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CustomerReviewCountReport()
        {
            List<Customer> customerList = new List<Customer>();
            foreach (Customer c in db.Customers)
            {
                customerList.Add(c);
            }

            foreach (Customer c in customerList)
            {
                var reviews = (from r in db.Reviews
                               where r.Author == c.FirstName
                               select r);

                foreach(Review r in reviews)
                {
                    c.Reviews.Add(r);
                }
                
            }



            var items = from c in customerList
                        orderby c.ReviewCount descending
                        select c;

            return View(items);
        }
    }
}
