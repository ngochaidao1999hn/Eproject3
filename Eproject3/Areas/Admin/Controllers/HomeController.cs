using Eproject3.Models;
using Eproject3.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eproject3.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseEntities db = new DatabaseEntities();
        Repository r = new Repository();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login");
            }
            Users user = (Users)Session["user"];
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", user.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", user.Roll_id);
            return View(db.Users.Find(user.id));
        }
        public ActionResult Login()
        {
            if (Session["isAdmin"] != null)
            {
                return RedirectToAction("Index");
            }
            if (TempData["done"] != null)
            {
                ViewBag.done = TempData["done"];
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string usn,string pwd)
        {
            string hashed = r.HashPwd(pwd);
            var isValid = db.Users.Where(p=>p.UPhone==usn && p.UPass== hashed).FirstOrDefault();
            ViewBag.usn = usn;
            ViewBag.pwd = pwd;
            if (isValid == null)
            {
                ViewBag.err = "Wrong credential";
                return View();
            }else if (isValid.Roll_id != 1 )
            {
                ViewBag.err = "You are not permited here";
                return View();
            }
            Session["user"] = isValid;
            Session["isAdmin"] = true;
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users)
        {
            var isValid = (Users)Session["user"];
            if (ModelState.IsValid)
            {
                users.Exp_Date = isValid.Exp_Date;
                users.Pack_id = isValid.Pack_id;
                users.Roll_id = isValid.Roll_id;
                db.Entry(users).State = EntityState.Modified;
                Session["user"] = users;
                await db.SaveChangesAsync();               
                return RedirectToAction("Index");
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }
    }
}