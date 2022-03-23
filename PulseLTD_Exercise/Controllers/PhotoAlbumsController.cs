using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ICSharpCode.SharpZipLib.Zip;
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

        [HttpPost]
        public ActionResult DownloadImages(int[] ids)
        {
            try
            {
                if (ids == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<PhotoAlbum> photoAlbums = new List<PhotoAlbum>();
                foreach (int id in ids) { photoAlbums.Add(db.PhotoAlbums.Find(id)); }

                //return File(photoAlbums[0].ImageText, "image/png");

                using (MemoryStream stream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < photoAlbums.Count; i++)
                        {
                            PhotoAlbum photoAlbum = photoAlbums[i];
                            var entry = archive.CreateEntry(photoAlbum.ImageName + ".jpg", CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                                zipStream.Write(photoAlbum.ImageText, 0, photoAlbum.ImageText.Length);
                            }
                        }
                    }
                    return File(stream.ToArray(), "application/zip", "Images.zip");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
        }

        // GET: PhotoAlbums/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
        public ActionResult PostImage(string title, string name)
        {
            if (Request.Files.Count > 0)
            {
                Console.WriteLine(Request.Files.ToString());
                HttpPostedFileBase postedFileBase = Request.Files[0];
                WebImage image = new WebImage(postedFileBase.InputStream);
                PhotoAlbum photoAlbum = new PhotoAlbum
                {
                    ImageTitle = title,
                    ImageName = name,
                    ImageDateTime = DateTime.Now,
                    ImageText = image.GetBytes()
                };
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
