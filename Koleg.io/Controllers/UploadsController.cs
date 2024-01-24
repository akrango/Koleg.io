using System;
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
    public class UploadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Uploads
        public ActionResult Index()
        {
            return View(db.Uploads.ToList());
        }

        [HttpGet]
        public ActionResult GetReviews(int id)
        {
            var reviews = db.Reviews.Where(r => r.UploadId == id).ToList();
            return PartialView("ReviewsPartial", reviews);
        }
        public ActionResult AddComment(int id)
        {
            Comment comment = new Comment();
            comment.UserId=User.Identity.GetUserId();
            comment.UploadId = id;
            comment.User = db.Users.Find(comment.UserId);
            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Explicitly load the upload entity without tracking
                var upload = db.Uploads.AsNoTracking().FirstOrDefault(u => u.Id == comment.UploadId);

                if (upload != null)
                {
                    // Associate the comment with the upload
                    comment.UploadId = upload.Id;
                    comment.User = db.Users.Find(comment.UserId);

                    // Add the comment to the upload's Comments collection
                    upload.Comments.Add(comment);
                    upload.IsCommentedOn = true;

                    // Attach the entities to the context (to avoid duplicate key tracking issues)
                    db.Entry(upload).State = EntityState.Unchanged;
                    db.Entry(comment.User).State = EntityState.Unchanged;

                    // Save changes
                    db.Comments.Add(comment);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = upload.Id });
                }
            }

            return View(comment);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddReview(ReviewPostModel reviewPostModel)
        {
            var userId = User.Identity.GetUserId();
            var upload = db.Uploads.Find(reviewPostModel.id);

            if (upload != null)
            {
                var review = new Review
                {
                    Rating = reviewPostModel.rating,
                    CommentText = reviewPostModel.commentText,
                    UserId = userId,
                    User = db.Users.Find(userId),
                    UploadId = reviewPostModel.id,
                    DateCreated = DateTime.Now
                };

                upload.Reviews.Add(review);
                upload.NumberOfRatingVotes++;
                upload.TotalSumOfRatings += reviewPostModel.rating;

                db.Reviews.Add(review);
                db.SaveChanges();
            }

            return View("Details", upload);
        }

        public ActionResult UserFiles()
        {
            // Get the currently logged-in user's ID
            string userId = User.Identity.GetUserId();

            // Retrieve files uploaded by the user
            List<Upload> userUploadedFiles = db.Uploads.Where(f => f.UserId == userId).ToList();

            return View(userUploadedFiles);
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(string description, HttpPostedFileBase file)
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

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    var uploadedFile = new Upload
                    {
                        FileName = file.FileName,
                        FileData = binaryReader.ReadBytes(file.ContentLength),
                        UserId = userId, // Set the UserId to associate the file with the user
                        Description = description // Set the description property
                    };

                    // Add the uploaded file to the database
                    db.Uploads.Add(uploadedFile);
                    var user=db.Users.FirstOrDefault(u=>u.Id == userId);
                    user.MyUploads.Add(uploadedFile);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            // If the model state is not valid, return the view with validation errors
            return View();
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



        // GET: Uploads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            UploadDetailsViewModel viewModel = new UploadDetailsViewModel();
            viewModel.Upload = upload;
            Comment comment=new Comment();
            var userId= User.Identity.GetUserId();
            comment.User = db.Users.FirstOrDefault(u => u.Id == userId);
            comment.UploadId = upload.Id;
            comment.UserId = userId;
            viewModel.Comment = comment;
            
            return View(upload);
        }

        // GET: Uploads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Uploads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FileName,FileData")] Upload upload)
        {
            if (ModelState.IsValid)
            {
                db.Uploads.Add(upload);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(upload);
        }

        // GET: Uploads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View("_EditUpload",upload);
        }

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileName,FileData")] Upload upload)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(upload).State = EntityState.Modified;
                var originalFile = db.Uploads.FirstOrDefault(f => f.Id == upload.Id);

                if (originalFile == null)
                {
                    return HttpNotFound();
                }

                // Update the file name in the database
                originalFile.FileName = upload.FileName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(upload);
        }
/*
        // GET: Uploads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // POST: Uploads/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Upload upload = db.Uploads.Find(id);
            db.Uploads.Remove(upload);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
*/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            db.Uploads.Remove(upload);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Uploads/Approve/5
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Upload upload = db.Uploads.Find(id);

            if (upload == null)
            {
                return HttpNotFound();
            }

            upload.IsApproved = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

   /*     // POST: Uploads/Approve/5
        [HttpPost, ActionName("Approve")]
        public ActionResult ApproveConfirmed(int id)
        {
            Upload upload = db.Uploads.Find(id);
            upload.IsApproved = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
*/
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
