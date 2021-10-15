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

namespace Eproject3.Areas.Admin.Controllers
{
    public class ExamsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Exams
        public async Task<ActionResult> Index()
        {
            var exams = db.Exams.Include(e => e.Contest).Include(e => e.Contester).Include(e => e.Recipes);
            return View(await exams.ToListAsync());
        }

        // GET: Exams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = await db.Exams.FindAsync(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            return View(exams);
        }
        public async Task<ActionResult> Marking(int? id,int mark)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = await db.Exams.FindAsync(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            exams.E_Status = 1;
            exams.Mark = mark;
            db.SaveChanges();
            return RedirectToAction("Contesters/" + exams.Contest_id, "Contesters");
        }
        // GET: Exams/Create
        public ActionResult Create()
        {
            ViewBag.Contest_id = new SelectList(db.Contest, "id", "id");
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name");
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title");
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Contest_id,Mark,Contester_id,Recipes_id,E_Status")] Exams exams)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exams);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Contest_id = new SelectList(db.Contest, "id", "id", exams.Contest_id);
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name", exams.Contester_id);
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", exams.Recipes_id);
            return View(exams);
        }

        // GET: Exams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = await db.Exams.FindAsync(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contest_id = new SelectList(db.Contest, "id", "id", exams.Contest_id);
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name", exams.Contester_id);
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", exams.Recipes_id);
            return View(exams);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Contest_id,Mark,Contester_id,Recipes_id,E_Status")] Exams exams)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exams).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Contest_id = new SelectList(db.Contest, "id", "id", exams.Contest_id);
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name", exams.Contester_id);
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", exams.Recipes_id);
            return View(exams);
        }

        // GET: Exams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = await db.Exams.FindAsync(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            return View(exams);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Exams exams = await db.Exams.FindAsync(id);
            db.Exams.Remove(exams);
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
