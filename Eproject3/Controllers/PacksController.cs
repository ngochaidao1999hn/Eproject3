using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eproject3.Models;

namespace Eproject3.Controllers
{
    public class PacksController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Packs
        public async Task<ActionResult> Index()
        {
            return View(await db.Packs.ToListAsync());
        }
        // GET: Packs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packs packs = await db.Packs.FindAsync(id);
            if (packs == null)
            {
                return HttpNotFound();
            }
            return View(packs);
        }

        // GET: Packs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Packs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name,price")] Packs packs)
        {
            if (ModelState.IsValid)
            {
                db.Packs.Add(packs);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(packs);
        }

        // GET: Packs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packs packs = await db.Packs.FindAsync(id);
            if (packs == null)
            {
                return HttpNotFound();
            }
            return View(packs);
        }

        // POST: Packs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name,price")] Packs packs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packs).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(packs);
        }

        // GET: Packs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packs packs = await db.Packs.FindAsync(id);
            if (packs == null)
            {
                return HttpNotFound();
            }
            return View(packs);
        }

        // POST: Packs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Packs packs = await db.Packs.FindAsync(id);
            db.Packs.Remove(packs);
            await db.SaveChangesAsync();
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
