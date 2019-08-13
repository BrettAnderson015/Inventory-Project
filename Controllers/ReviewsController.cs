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
    public class ReviewsController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Reviews
        public ActionResult Index(string sortOrder)
        {
            ViewBag.ItemIdSortParam = String.IsNullOrEmpty(sortOrder) ? "ItemId_desc" : "";
            ViewBag.ItemNameSortParam = sortOrder == "ItemName" ? "ItemName_desc" : "ItemName";
            ViewBag.AuthorSortParam = sortOrder == "Author" ? "Author_desc" : "Author";
            var reviews = from r in db.Reviews
                          select r;
            switch (sortOrder)
            {
                case "ItemId_desc":
                    reviews = reviews.OrderByDescending(r => r.ItemId);
                    break;
                case "ItemName":
                    reviews = reviews.OrderBy(r => r.ItemName);
                    break;
                case "ItemName_desc":
                    reviews = reviews.OrderByDescending(r => r.ItemName);
                    break;
                case "Author":
                    reviews = reviews.OrderBy(r => r.Author);
                    break;
                case "Author_desc":
                    reviews = reviews.OrderByDescending(r => r.Author);
                    break;
                default:
                    reviews = reviews.OrderBy(r => r.ItemId);
                    break;
            }
            return View(reviews.ToList());
        }

        public ActionResult ItemReviews(int itemId, string customer)
        {
            ItemCustomerReview itemCustomerReview = new ItemCustomerReview();
            itemCustomerReview.Item = db.Items.Where(i => i.Id == itemId).SingleOrDefault();
            Customer customer1 = new Customer();
            customer1.FirstName = customer;
            itemCustomerReview.currentCustomer = customer1;
            itemCustomerReview.Reviews = db.Reviews.Where(r => r.ItemId == itemId);
            return View(itemCustomerReview);
        }

        public ActionResult Add(int itemId, string currentCustomer, string itemName)
        {
            Review review = new Review();
            review.ItemId = itemId;
            review.ItemName = itemName;
            review.Author = currentCustomer;
            return View(review);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Id,Author,Content,Date,ItemId,ItemName")]Review review)
        {
            
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("ItemReviews", new { itemId = review.ItemId,customer = review.Author});
            }
            return RedirectToAction("ItemReviews", new { itemId = review.ItemId, customer = review.Author });
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Author,Content,Date")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Author,Content,Date")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
