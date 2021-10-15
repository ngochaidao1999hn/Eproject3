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
    public class RecipesController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Recipes
        public async Task<ActionResult> Index()
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var recipes = db.Recipes.Include(r => r.Users);
            return View(await recipes.ToListAsync());
        }
        // GET: Recipes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Lay feedbacks
            ViewBag.FeedBack = db.FeedBack.Where(m => m.Recipes_id == id).Include(i => i.Users).ToList();
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }
        /// <summary>
        /// phan cu hai
        /// </summary>
        /// <returns></returns>
        // GET: Recipes/Create
        public ActionResult Create()
        {
            Users u = (Users)Session["User"];
            if (u != null)
            {
                ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                return View();
            }
            return RedirectToAction("LoginView", "Users");
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status,Cate_id")] Recipes recipes, HttpPostedFileBase[] Url, string[] txtText, string[] txtIgredent, int txtStatus, string rate)
        {
            int flag = 0;
            string Cont = "";
            string url_img = "";
            string ingre = "";
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (HttpPostedFileBase img in Url)
                    {
                        if (img != null)
                        {

                            string ex = Path.GetExtension(img.FileName);
                            if (!check(ex, formats))
                            {
                                flag = 1;
                                ViewBag.FileStatus = ex + " is not an image";
                                ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                                return View(recipes);
                            }
                            url_img += Path.GetFileName(img.FileName) + "$";
                        }
                        else
                        {
                            flag = 1;
                            ViewBag.FileStatus = "Content must have image !!!!";
                            ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                            return View(recipes);

                        }
                        if (flag != 1)
                        {
                            string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }

                recipes.Img = url_img.Substring(0, url_img.Length - 1);

                foreach (var text in txtText)
                {
                    if (text != "")
                    {

                        Cont += text + "$";
                    }
                }
                foreach (var ingredent in txtIgredent)
                {
                    if (ingredent != "")
                    {

                        ingre += ingredent + "$";
                    }
                }
                Cont = Cont.Substring(0, Cont.Length - 1);
                recipes.Content = Cont;
                recipes.ingredent = ingre.Substring(0, ingre.Length - 1);
                if (Session["user"] != null)
                {
                    var isvalid = (Users)Session["user"];
                    recipes.Contester_id = isvalid.id;
                }
                else
                {
                    recipes.Contester_id = db.Users.Where(p => p.UPhone == "000").FirstOrDefault().id;
                }
                if (flag != 1)
                {
                    recipes.Levels = rate;
                    recipes.R_Status = txtStatus;
                    db.Recipes.Add(recipes);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                    return View(recipes);
                }               
            }
            ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
            return View(recipes);
        }
        // GET: Recipes/Edit/5
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
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
            return View(recipes);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Title,Content,Img,Contester_id,R_Status,Cate_id")] Recipes recipes, HttpPostedFileBase[] Url, string[] txtText, string[] txtIgredent, int txtStatus, string rate)
        {
            int flag = 0;
            string Cont = "";
            string url_img = "";
            string ingre = "";
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (HttpPostedFileBase img in Url)
                    {
                        if (img != null)
                        {
                           
                            string ex = Path.GetExtension(img.FileName);
                            if (!check(ex, formats))
                            {
                                flag = 1;
                                ViewBag.FileStatus = ex + " is not an image";
                                ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                                return View(recipes);
                            }
                            url_img += Path.GetFileName(img.FileName) + "$";
                            recipes.Img = url_img.Substring(0, url_img.Length - 1);
                        }
                        if (flag != 1)
                        {
                            string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                        }
                    }


                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }


                foreach (var text in txtText)
                {
                    if (text != "")
                    {

                        Cont += text + "$";
                    }
                }
                foreach (var ingredent in txtIgredent)
                {
                    if (ingredent != "")
                    {

                        ingre += ingredent + "$";
                    }
                }
                if (Session["user"] != null)
                {
                    var isvalid = (Users)Session["user"];
                    recipes.Contester_id = isvalid.id;
                }
                if (flag != 1)
                {
                    recipes.Levels = rate;
                    Cont = Cont.Substring(0, Cont.Length - 1);
                    recipes.Content = Cont;
                    recipes.ingredent = ingre.Substring(0, ingre.Length - 1);
                    recipes.R_Status = txtStatus;
                    db.Entry(recipes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
                    return View(recipes);
                }
            }
            ViewBag.Cate_id = new SelectList(db.Categories.ToList(), "id", "Cate_name");
            return View(recipes);
        }
            // GET: Recipes/Delete/5
            public async Task<ActionResult> Delete(int? id)
        {
            if (Session["isAdmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = await db.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recipes recipes = await db.Recipes.FindAsync(id);
            db.Recipes.Remove(recipes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public bool check(string extension, string[] format) {
            foreach (string exten in format)
            {
                if (extension.Contains(exten))
                {
                    return true;
                }
            }
            return false;
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
