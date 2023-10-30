using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koleg.io.Models;

namespace Koleg.io.Controllers
{
    public class UploadChunksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UploadChunks
        public ActionResult Index()
        {
            var uploadChunks = db.UploadChunks.Include(u => u.File);
            return View(uploadChunks.ToList());
        }

        // GET: UploadChunks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UploadChunk uploadChunk = db.UploadChunks.Find(id);
            if (uploadChunk == null)
            {
                return HttpNotFound();
            }
            return View(uploadChunk);
        }

        // GET: UploadChunks/Create
        public ActionResult Create()
        {
            ViewBag.FileId = new SelectList(db.Uploads, "Id", "FileName");
            return View();
        }

        // POST: UploadChunks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ChunkData,FileId")] UploadChunk uploadChunk)
        {
            if (ModelState.IsValid)
            {
                db.UploadChunks.Add(uploadChunk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FileId = new SelectList(db.Uploads, "Id", "FileName", uploadChunk.FileId);
            return View(uploadChunk);
        }

        // GET: UploadChunks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UploadChunk uploadChunk = db.UploadChunks.Find(id);
            if (uploadChunk == null)
            {
                return HttpNotFound();
            }
            ViewBag.FileId = new SelectList(db.Uploads, "Id", "FileName", uploadChunk.FileId);
            return View(uploadChunk);
        }

        // POST: UploadChunks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ChunkData,FileId")] UploadChunk uploadChunk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uploadChunk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FileId = new SelectList(db.Uploads, "Id", "FileName", uploadChunk.FileId);
            return View(uploadChunk);
        }

        // GET: UploadChunks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UploadChunk uploadChunk = db.UploadChunks.Find(id);
            if (uploadChunk == null)
            {
                return HttpNotFound();
            }
            return View(uploadChunk);
        }

        // POST: UploadChunks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UploadChunk uploadChunk = db.UploadChunks.Find(id);
            db.UploadChunks.Remove(uploadChunk);
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
