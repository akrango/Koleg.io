﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koleg.io.Models;
using Microsoft.AspNet.Identity;

namespace Koleg.io.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects
        public ActionResult Index(int id)
        {
            ViewBag.Id = id;
            return View(db.Subjects.Include(s => s.Uploads).ToList());
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            // Perform a search in your database based on the searchTerm
            var subjects = db.Subjects
                .Where(s => s.Name.Contains(searchTerm) || s.Semester.Contains(searchTerm))
                .ToList();

            return PartialView("_SearchResults", subjects);
        }

        public ActionResult DownloadFile(int id)
        {
            // Retrieve the uploaded file from the database by its ID
            Upload uploadedFile = db.Uploads.FirstOrDefault(f => f.Id == id);

            if (uploadedFile != null)
            {
                // Prepare the file content for download
                byte[] fileData = uploadedFile.FileData;
                string fileName = uploadedFile.FileName;

                // Return the file as a download response
                return File(fileData, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                // Handle the case where the file with the given ID was not found
                return HttpNotFound();
            }
        }

        public ActionResult UploadFile(int id)
        {
            SubjectUploadViewModel model=new SubjectUploadViewModel();
            model.SubjectId = id;
            model.Subject = db.Subjects.Find(id);
           
            return PartialView("_CreateUpload",model);
        }

        [HttpPost]
        public ActionResult UploadFile(string description, HttpPostedFileBase file, SubjectUploadViewModel model)
        {
            if (file == null || file.ContentLength == 0)
            {
                // Handle the case where no file is selected for upload
                ModelState.AddModelError("file", "Please select a file.");
            }

            if (string.IsNullOrEmpty(description))
            {
                // Handle the case where the description is empty
                ModelState.AddModelError("description", "Please enter a description.");
            }

            if (ModelState.IsValid)
            {
                // Get the currently logged-in user's ID (you may need to customize this based on your authentication setup)
                string userId = User.Identity.GetUserId();
                var subject = db.Subjects.Find(model.SubjectId);

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    var uploadedFile = new Upload
                    {
                        FileName = file.FileName,
                        FileData = binaryReader.ReadBytes(file.ContentLength),
                        UserId = userId, // Set the UserId to associate the file with the user
                        Description = description, // Set the description property
                    };

                    // Add the uploaded file to the database
                    db.Uploads.Add(uploadedFile);
                    subject.Uploads.Add(uploadedFile);
                    uploadedFile.Subject = subject;
                    db.SaveChanges();
                }
                /*return PartialView("_ShowSuccessUpload");*/
                ViewBag.AddedFile = 1;
/*                Subject subject = db.Subjects.Include(s => s.Uploads).FirstOrDefault(s => s.Id == id);
*/                if (subject == null)
                {
                    return HttpNotFound();
                }
                return View("Details", subject);
                /*return View("Details", new { id = subject.Id });*/
/*                return RedirectToAction("ShowReviewModal");
*/            }

            // If the model state is not valid, return the view with validation errors
            return PartialView("_CreateUpload", model);
        }

        [HttpGet]
        public ActionResult ShowReviewModal()
        {
            return PartialView("_ShowSuccessUpload");
        }


        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Include(s => s.Uploads).FirstOrDefault(s => s.Id == id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddedFile = 0;

            if (subject.Uploads.Any())
            {
                ViewBag.OverallScore = subject.Uploads
                    .Where(upload => upload.IsApproved)
                    .SelectMany(upload => upload.Reviews)
                    .DefaultIfEmpty()
                    .Average(review => review != null ? review.Rating : 0);
            }
            else
            {
                ViewBag.OverallScore = 0; // Set a default value if there are no reviews
            }

            // Calculate total number of files
            ViewBag.TotalNumberOfFiles = subject.Uploads
                .Where(upload => upload.IsApproved==true)
                .Count();

            // Find the most active user
            ApplicationUser mostActiveUser = subject.Uploads
             .Where(upload => upload.IsApproved)
             .GroupBy(upload => upload.UserId)
             .OrderByDescending(group => group.Count())
             .Select(group => group.Key)
             .Select(userId => subject.Uploads.FirstOrDefault(upload => upload.UserId == userId)?.User)
             .FirstOrDefault();


            ViewBag.MostActiveUser = mostActiveUser;

            // Number of uploads by the most active user
            if(mostActiveUser != null)
            {
                ViewBag.MostActiveUserUploadCount = subject.Uploads
                .Count(u => u.UserId == mostActiveUser.Id);
            }

            return View(subject);
        }
        public List<string> Semesters = new List<string>()
        {
            "Зимски",
            "Летен"
        };
        // GET: Subjects/Create
        public ActionResult Create()
        {
            ViewBag.Semesters = new SelectList(Semesters);
            return PartialView("_CreateSubject");

        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Year,Semester")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = 0 });
            }
            else
            {
                ViewBag.Semesters = new SelectList(Semesters);
                return PartialView("_CreateSubject", subject);
            }
        }

        public ActionResult ShowSuccessUpload()
        {
            return PartialView("_ShowSuccessUpload");
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Include(s => s.Uploads).FirstOrDefault(s => s.Id == id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.Semesters = new SelectList(Semesters);
            return PartialView("_EditSubject",subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Year,Semester")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new {id=0});
            }
            return View(subject);
        }

        /*// GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Include(s => s.Uploads).FirstOrDefault(s => s.Id == id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Include(s => s.Uploads).FirstOrDefault(s => s.Id == id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        public ActionResult Delete(int? id)
        {
            Subject subject = db.Subjects.Include(s => s.Uploads).FirstOrDefault(s => s.Id == id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("Index", new {id=0});
        }

        public ActionResult SetAddedFileToZero()
        {
            ViewBag.AddedFile = 0;
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
