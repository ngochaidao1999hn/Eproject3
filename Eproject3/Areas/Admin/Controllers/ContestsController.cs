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
using System.IO;

namespace Eproject3.Areas.Admin.Controllers
{
    public class ContestsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Contests
        public async Task<ActionResult> Index()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(await db.Contest.OrderByDescending(p=>p.id).ToListAsync());
        }

        // GET: Contests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = await db.Contest.FindAsync(id);
            if (contest == null)
            {                
                return HttpNotFound();
            }
            return View(contest);
        }

        // GET: Contests/Create
        public ActionResult Create()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Contests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,title,requirement,C_Time,exp_time,C_Description,img")] Contest contest, HttpPostedFileBase Url)
        {
            if (DateTime.Compare(contest.C_Time.Value,contest.exp_time.Value) >0)
            {
                ViewBag.Soon = "Start date can not be earlier than end date ";
                return View(contest);
            }
            string url_img = "";
            if (ModelState.IsValid)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url.FileName));
                    Url.SaveAs(path);
                    url_img += Path.GetFileName(Url.FileName) + ",";
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
                contest.img = url_img.Substring(0, url_img.Length - 1);
                db.Contest.Add(contest);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contest);
        }

        // GET: Contests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = await db.Contest.FindAsync(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,title,requirement,C_Time,exp_time,C_Description,img,id_winner")] Contest contest, HttpPostedFileBase Url)
        {
            if (DateTime.Compare(contest.C_Time.Value, contest.exp_time.Value) > 0)
            {
                
                ViewBag.Soon = "Start date can not be earlier than end date ";
                return View(contest);
            }
            if (ModelState.IsValid)
            {
                string url_img = "";
                try
                {
                    string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url.FileName));
                    Url.SaveAs(path);
                    url_img += Path.GetFileName(Url.FileName) + ",";
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
                if (Url != null)
                {
                    contest.img = url_img.Substring(0, url_img.Length - 1);
                }
                db.Entry(contest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contest);
        }

        // GET: Contests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (TempData["cas"] != null)
            {
                ViewBag.cas = "There are contestants in this contest,can not drop";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = await db.Contest.FindAsync(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (db.Contester.Where(p=>p.Contest_id==id).Count() >0)
            {
                TempData["cas"] = true;
                return RedirectToAction("Delete/"+id);
            }
            Contest contest = await db.Contest.FindAsync(id);
            db.Contest.Remove(contest);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> ForceDelete(int id)
        {
            db.Exams.ToList().RemoveAll(p=>p.Contest_id==id);
            db.Contester.ToList().RemoveAll(p=>p.Contest_id==id);
            Contest contest = await db.Contest.FindAsync(id);
            db.Contest.Remove(contest);
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
