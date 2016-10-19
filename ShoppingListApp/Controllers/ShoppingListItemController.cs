using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using ShoppingListApp.Models;

namespace ShoppingListApp.Controllers
{
    [Authorize]
    public class ShoppingListItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingListItem
        public ActionResult Index()
        {
            var shoppingListItems = db.ShoppingListItems.Include(s => s.ShoppingList);
            return View(shoppingListItems.ToList());
        }

        public ActionResult ItemsByList(int id)
        {

            var shoppingListItems = db.ShoppingListItems.Include(s => s.ShoppingList).Where(l => l.ShoppingListId == id);
            ViewBag.ShoppingListName = db.ShoppingLists.Find(id).Name;
            return View(shoppingListItems.ToList());
        }

        [HttpPost]
        public ActionResult ItemsByList(FormCollection formCollection)
        {
            string[] ids = formCollection["itemId"].Split(new char[] { ',' });
            foreach (string id in ids)
            {
                var item = this.db.ShoppingListItems.Find(int.Parse(id));
                this.db.ShoppingListItems.Remove(item);
                this.db.SaveChanges();
            }
            return RedirectToAction("ItemsByList");
        }

        // GET: ShoppingListItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
        }

        // GET: ShoppingListItem/Create
        public ActionResult Create()
        {
            ViewBag.ShoppingListId = new SelectList(db.ShoppingLists, "Id", "Name");
            return View();
        }

        // POST: ShoppingListItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShoppingListId,Contents,Note,Priority,IsChecked,CreatedUtc,ModifiedUtc")] ShoppingListItem shoppingListItem)
        {
            if (ModelState.IsValid)
            {
                shoppingListItem.CreatedUtc = DateTime.UtcNow;
                shoppingListItem.ModifiedUtc = DateTime.UtcNow;
                db.ShoppingListItems.Add(shoppingListItem);
                db.SaveChanges();
                return RedirectToAction("Index", "ShoppingList");
            }

            ViewBag.ShoppingListId = new SelectList(db.ShoppingLists, "Id", "Name", shoppingListItem.ShoppingListId);
            return View(shoppingListItem);
        }

        // GET: ShoppingListItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShoppingListId = new SelectList(db.ShoppingLists, "Id", "Name", shoppingListItem.ShoppingListId);
            return View(shoppingListItem);
        }

        // POST: ShoppingListItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShoppingListId,Contents,Note,Priority,IsChecked,CreatedUtc,ModifiedUtc")] ShoppingListItem shoppingListItem)
        {
            if (ModelState.IsValid)
            {
                var currentListItem = db.ShoppingListItems.AsNoTracking().Where(c => c.Id == shoppingListItem.Id).FirstOrDefault();
                shoppingListItem.CreatedUtc = currentListItem.CreatedUtc;
                shoppingListItem.ModifiedUtc = DateTime.UtcNow;
                db.Entry(shoppingListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "ShoppingList");
            }
            ViewBag.ShoppingListId = new SelectList(db.ShoppingLists, "Id", "Name", shoppingListItem.ShoppingListId);
            return View(shoppingListItem);
        }

        // GET: ShoppingListItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
        }

        // POST: ShoppingListItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            db.ShoppingListItems.Remove(shoppingListItem);
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
