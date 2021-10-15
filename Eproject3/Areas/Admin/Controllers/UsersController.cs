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
    public class UsersController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        Repo.Repository r = new Repo.Repository();
        // GET: Users
        public async Task<ActionResult> Index()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var users = db.Users.Include(u => u.Packs).Include(u => u.Roles);
            return View(await users.ToListAsync());
        }
        public async Task<ActionResult> LogOut()
        {
            Session["user"] = null;
            Session["isAdmin"] = null;
            return RedirectToAction("index", "Home");
        }
        // GET: Users/Create
        public ActionResult Create()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name");
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users, HttpPostedFileBase Url)
        {
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            string url_img = "";
            if (ModelState.IsValid)
            {
                if (Url != null)
                {
                    string path = "";
                    if (db.Users.Where(p => p.UPhone == users.UPhone).FirstOrDefault() != null)
                    {
                        ViewBag.ExErr = "This phone number has been registered before";
                        return View(users);
                    }
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(Url.FileName));
                        url_img += Path.GetFileName(Url.FileName) + ",";
                    }
                    catch (Exception e)
                    {
                        ViewBag.FileStatus = "Error while file uploading.";
                    }
                    string ex = Path.GetExtension(Url.FileName);
                    if (!r.check(ex.ToLower(), formats))
                    {
                        ViewBag.FileStatus = ex + " is not an image";
                        return View(users);
                    }
                    users.Img = url_img.Substring(0, url_img.Length - 1);
                    if (users.Pack_id == 1)
                    {
                        users.Exp_Date = DateTime.Now.AddMonths(1);
                    }
                    else if (users.Pack_id == 2)
                    {
                        users.Exp_Date = DateTime.Now.AddYears(1);
                    }
                    else
                    {
                        users.Exp_Date = DateTime.Now;
                    }
                    Url.SaveAs(path);
                    string hashed = r.HashPwd(users.UPass);
                    users.UPass = hashed;
                    //users.Roll_id = 2;
                    db.Users.Add(users);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }              
            }
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }
        public ActionResult ChangePhoto()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePhoto(HttpPostedFileBase Url)
        {
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            string url_img = "";
            var users = (Users)Session["user"];
            if (users != null)
            {
                if (Url != null)
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
                    string ex = Path.GetExtension(Url.FileName);

                    if (!r.check(ex.ToLower(), formats))
                    {
                        ViewBag.FileStatus = ex + " is not an image";
                        return View();
                    }
                    users.Img = url_img.Substring(0, url_img.Length - 1);
                    var u = db.Users.Find(users.id);
                    u.Img = users.Img;
                    db.SaveChanges();
                    Session["user"] = users;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.FileStatus = "You must upload an image";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginView");
            }
        }



        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login","Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,UPhone,UPass,UAdress,Img,Roll_id,Pack_id,Exp_Date,AccNum")] Users users)
        {
            var isValid = (Users)Session["user"];
            if (ModelState.IsValid)
            {
                users.Exp_Date = isValid.Exp_Date;
                users.Pack_id = isValid.Pack_id;
                users.Roll_id = isValid.Roll_id;
                db.Entry(users).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Pack_id = new SelectList(db.Packs, "id", "name", users.Pack_id);
            ViewBag.Roll_id = new SelectList(db.Roles, "id", "name", users.Roll_id);
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login");
            }
            if (TempData["cass"] != null)
            {
                ViewBag.cass = "This user posted tips or recipes can not drop";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (db.Recipes.Where(p => p.Contester_id == id).Count()>0 || db.Tips.Where(p => p.Use_id == id).Count()>0)
            {
                TempData["cass"] = true;
                return RedirectToAction("Delete/"+id);
            }
            Users users = await db.Users.FindAsync(id);
            db.Users.Remove(users);
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
        public ActionResult ChangePwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePwd(string oldp, string newp)
        {
            var user = (Users)Session["user"];
            string hashed = r.HashPwd(oldp);
            var isvalid = db.Users.Where(p => p.UPhone == user.UPhone && p.UPass == hashed).FirstOrDefault();
            ViewBag.old = oldp;
            ViewBag.newp = newp;
            if (user != null && isvalid != null)
            {
                if (newp.Length < 8 || newp.Length > 50)
                {
                    ViewBag.err = "Password must be a 8-50 characters string ";
                    return View();
                }
                isvalid.UPass = r.HashPwd(newp);
                db.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            ViewBag.err = "Wrong credential";
            return View();
        }
        public ActionResult ForgetPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwd(string phone)
        {
            ViewBag.phone = phone;
            var isvalid = db.Users.Where(p => p.UPhone == phone).FirstOrDefault();
            if (isvalid != null)
            {
                Guid newPass = new Guid();
                isvalid.UPass = r.HashPwd(newPass.ToString());
                //Send sms to User to send new password
                db.SaveChanges();
                TempData["done"] = "Check your inbox in your phone to receive new password ";
                return RedirectToAction("Login","Home");
            }
            else
            {
                ViewBag.err = "No phone number found";
                return View();
            }
        }
    }
}
