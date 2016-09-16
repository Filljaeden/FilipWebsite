using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilipWebsite.Models;

namespace FilipWebsite.Controllers
{
    [Authorize]
    public class RecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipes
        public ActionResult Index()
        {
            var recipes = db.Recipes.Include(r => r.Author).Include(r => r.Category);
            return View(recipes.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            //Recipe student = db.Recipes.Include(s => s.Files).SingleOrDefault(s => s.Id == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Names");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,AuthorId,CategoryId")] Recipe recipe, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var avatar = new Models.File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        recipe.Files = new List<Models.File> { avatar };
                    }
                    recipe.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    db.Recipes.Add(recipe);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", recipe.AuthorId);
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Names", recipe.CategoryId);
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            //Recipe student = db.Recipes.Include(s => s.Files).SingleOrDefault(s => s.Id == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", recipe.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Names", recipe.CategoryId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,Body,AuthorId,CategoryId")] Recipe recipe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(recipe).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", recipe.AuthorId);
        //    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Names", recipe.CategoryId);
        //    return View(recipe);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recipesToUpdate = db.Recipes.Find(id);
            if (TryUpdateModel(recipesToUpdate, "",
                new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (recipesToUpdate.Files.Any(f => f.FileType == FileType.Avatar))
                        {
                            db.Files.Remove(recipesToUpdate.Files.First(f => f.FileType == FileType.Avatar));
                        }
                        var avatar = new Models.File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        recipesToUpdate.Files = new List<Models.File> { avatar };
                    }
                    db.Entry(recipesToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(recipesToUpdate);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            db.Recipes.Remove(recipe);
            db.SaveChanges();
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
