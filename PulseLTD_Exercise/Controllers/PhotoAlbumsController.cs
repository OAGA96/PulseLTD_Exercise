using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PulseLTD_Exercise.Models;

namespace PulseLTD_Exercise.Controllers
{
    public class PhotoAlbumsController : Controller
    {
        private PulseLTDEntities db = new PulseLTDEntities();

        // GET: PhotoAlbums
        public ActionResult Index()
        {
            return View(db.PhotoAlbums.ToList());
        }

        // GET: PhotoAlbums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);
            if (photoAlbum == null)
            {
                return HttpNotFound();
            }
            return View(photoAlbum);
        }

        // GET: PhotoAlbums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoAlbums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImageId,ImageTitle,ImageName,ImageText,ImageDateTime")] PhotoAlbum photoAlbum, HttpPostedFileBase file)
        {
            //HttpPostedFileBase photoFileBase =Request.Files[0];

            WebImage image = new WebImage(file.InputStream);

            photoAlbum.ImageText = image.GetBytes();

            if (ModelState.IsValid)
            {
                db.PhotoAlbums.Add(photoAlbum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photoAlbum);
        }

        [HttpPost]
        public ActionResult PostImage(string title, string name/*, HttpPostedFileBase file*/)
        {
            if (Request.Files.Count > 0)
            {
                Console.WriteLine(Request.Files.ToString());
                HttpPostedFileBase postedFileBase = Request.Files[0];
                WebImage image = new WebImage(postedFileBase.InputStream);
                PhotoAlbum photoAlbum = new PhotoAlbum();
                photoAlbum.ImageTitle = title;
                photoAlbum.ImageName = name;
                photoAlbum.ImageDateTime = DateTime.Now;
                photoAlbum.ImageText = image.GetBytes();
                if (ModelState.IsValid)
                {
                    db.PhotoAlbums.Add(photoAlbum);
                    db.SaveChanges();
                    return Json("success");
                }
                return Json("");
            }
            else return Json("No files to upload");
        }

        // GET: PhotoAlbums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);
            if (photoAlbum == null)
            {
                return HttpNotFound();
            }
            return View(photoAlbum);
        }

        // POST: PhotoAlbums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImageId,ImageTitle,ImageName,ImageText,ImageDateTime")] PhotoAlbum photoAlbum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photoAlbum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photoAlbum);
        }


        // GET: PhotoAlbums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);
            if (photoAlbum == null)
            {
                return HttpNotFound();
            }
            return View(photoAlbum);
        }

        // POST: PhotoAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);
            db.PhotoAlbums.Remove(photoAlbum);
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
