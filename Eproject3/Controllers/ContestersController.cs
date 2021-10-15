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
    public class ContestersController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Contesters
        public async Task<ActionResult> Index()
        {
            var contester = db.Contester.Include(c => c.Users);
            return View(await contester.ToListAsync());
        }
        // GET: Contesters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contester contester = await db.Contester.FindAsync(id);
            if (contester == null)
            {
                return HttpNotFound();
            }
            return View(contester);
        }

        // GET: Contesters/Create
        public ActionResult Create()
        {
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone");
            return View();
        }

        // POST: Contesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Use_id,Name,Phone")] Contester contester)
        {
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            if (ModelState.IsValid)
            {
                int ctId = (int)TempData["ctId"];
                if (Session["user"] != null )
                {
                    var isValid = (Users)Session["user"];
                    contester.Use_id = isValid.id;
                    contester.Phone = isValid.UPhone;
                    contester.Name = isValid.UAdress;
                }
                else {
                    contester.Use_id = db.Users.Where(p => p.UPhone == "000").FirstOrDefault().id;
                }
                if (db.Contester.Where(p => p.Phone == contester.Phone && p.Contest_id== ctId).FirstOrDefault() != null)
                {
                    TempData["exist"] = "This phone number has been registered before,Pls try another one";
                    return RedirectToAction("Details/"+ ctId, "Contests");
                }
                contester.Contest_id = (int)TempData["ctId"];
                db.Contester.Add(contester);
                await db.SaveChangesAsync();
                TempData["cterId"] = contester.id;
                TempData["ctID"] = contester.Contest_id;
                return RedirectToAction("Create","Exams");
            }
            return View(contester);
        }
        public ActionResult Regist4Contest(int Contest_id)
        {
            ViewBag.Contest_id = Contest_id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regist4Contest([Bind(Include = "id,Use_id,Name,Phone")] Contester contester, int Contest_id) {
            if (ModelState.IsValid)
            {
                contester.Contest_id = Contest_id;
                db.Contester.Add(contester);
                db.SaveChangesAsync();
                Session["Contest_id"] = contester.id;
                return Redirect("~/Recipes/Create");
            }

            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            return View(contester);
        }
        // GET: Contesters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contester contester = await db.Contester.FindAsync(id);
            if (contester == null)
            {
                return HttpNotFound();
            }
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            return View(contester);
        }

        // POST: Contesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Use_id,Name,Phone")] Contester contester)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contester).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", contester.Use_id);
            return View(contester);
        }

        // GET: Contesters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contester contester = await db.Contester.FindAsync(id);
            if (contester == null)
            {
                return HttpNotFound();
            }
            return View(contester);
        }

        // POST: Contesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contester contester = await db.Contester.FindAsync(id);
            db.Contester.Remove(contester);
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
