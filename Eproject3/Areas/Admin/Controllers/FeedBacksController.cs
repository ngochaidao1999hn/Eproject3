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
    public class FeedBacksController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: FeedBacks
        public async Task<ActionResult> Index(int id,int type)
        {
            IQueryable<FeedBack> feedBack = null;
            if (type==0)
            {
                 feedBack = db.FeedBack.Where(p=>p.Recipes_id==id).Include(f => f.Recipes).Include(f => f.Tips).Include(f => f.Users);
            }
            else
            {
                feedBack = db.FeedBack.Where(p=>p.Tip_id==id).Include(f => f.Recipes).Include(f => f.Tips).Include(f => f.Users);
            }
            return View(await feedBack.ToListAsync());
        }
        
        // GET: FeedBacks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Create
        public ActionResult Create()
        {
            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title");
            ViewBag.Tip_id = new SelectList(db.Tips, "id", "Content");
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone");
            return View();
        }

        // POST: FeedBacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Use_id,Recipes_id,Content,Tip_id")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                db.FeedBack.Add(feedBack);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Recipes_id = new SelectList(db.Recipes, "id", "Title", feedBack.Recipes_id);
            ViewBag.Tip_id = new SelectList(db.Tips, "id", "Content", feedBack.Tip_id);
            ViewBag.Use_id = new SelectList(db.Users, "id", "UPhone", feedBack.Use_id);
            return View(feedBack);
        }

        // GET: FeedBacks/Edit/5
      

        // GET: FeedBacks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeedBack feedBack = await db.FeedBack.FindAsync(id);
            db.FeedBack.Remove(feedBack);
            await db.SaveChangesAsync();
            return RedirectToAction("Index","Recipes");
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
