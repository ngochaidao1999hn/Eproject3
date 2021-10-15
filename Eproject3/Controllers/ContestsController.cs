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
using PagedList;
namespace Eproject3.Controllers
{
    public class ContestsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Contests
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            DateTime next7days = DateTime.Today.AddDays(7);
            var contest = db.Contest.OrderByDescending(p => p.id).ToList();
            return View(contest.ToPagedList(pageNumber, pageSize));
        }

        // GET: Contests/Details/5
        public async Task<ActionResult> Details(int? id,int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.exams = db.Exams.Where(p=>p.Contest_id==id).OrderByDescending(p=>p.Mark).ToList().ToPagedList(pageNumber,pageSize);
            if (TempData["over"] != null)
            {
                ViewBag.over = TempData["over"];
            }
            if (TempData["early"] != null)
            {
                ViewBag.early = TempData["early"];
            }
            if (TempData["exist"] != null)
            {
                ViewBag.exist = TempData["exist"];
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
        public ActionResult Join(int id)
        {
            var isvalid = db.Contest.Find(id);
            if (isvalid.exp_time<DateTime.Now)
            {
                TempData["over"] = "This contest is over";
                return RedirectToAction("Details/" + id);
            }else if (isvalid.C_Time>DateTime.Now)
            {
                TempData["early"] = "This contest has not begun yet";
                return RedirectToAction("Details/" + id);
            }
            TempData["ctId"] = id;           
            return RedirectToAction("Create","Contesters");
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
