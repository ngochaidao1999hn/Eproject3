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

        // GET: Exams/Create
        public ActionResult Create()
        {
            ViewBag.Contest_id = new SelectList(db.Contest, "id", "id");
            ViewBag.Contester_id = new SelectList(db.Contester, "id", "Name");
            if (Session["user"] != null)
            {
                //int cterId = (int)TempData["cterId"];
                var isvalid = (Users)Session["user"];
                var recies = db.Recipes.Where(p => p.Contester_id == isvalid.id);
                if (recies.Count() >0)
                {
                    ViewBag.Recipes_id = new SelectList(recies, "id", "Title");
                }
                return View();
            }
            //giai quyet tai sao de ko dang nhao cx hien ra select list
            if (TempData["reId"] != null)
            {
                int reID = (int)TempData["reId"];
                ViewBag.Recipes_id = new SelectList(db.Recipes.Where(p => p.id == reID), "id", "Title");
            }
            return View();
        }
        public ActionResult Supplement()
        {
            TempData["Supplement"] = true;
            return RedirectToAction("Create","Recipes");
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
                TempData.Keep("cterId");
                exams.Contester_id =(int)TempData["cterId"];
                exams.Contest_id = (int)TempData["ctID"];
                exams.E_Status = 0;
                exams.Mark = 0;
                db.Exams.Add(exams);
                await db.SaveChangesAsync();
                TempData["done"] = true;
                return RedirectToAction("Index","Home");
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
