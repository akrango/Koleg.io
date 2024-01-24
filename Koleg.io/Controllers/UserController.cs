﻿using Koleg.io.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Koleg.io.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> _userManager; // Assuming you have a custom ApplicationUser class
        private ApplicationDbContext db = new ApplicationDbContext();

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public UserController() { }


        // GET: /Upload/Index
        public ActionResult Index()
        {
            ApplicationUser u = db.Users.Find(User.Identity.GetUserId());
            ViewBag.ActiveUser = u;
            return View();
        }

        // POST: /Upload/UploadProfilePicture
        [HttpPost]
        public ActionResult UploadProfilePicture(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    ApplicationUser u = db.Users.Find(User.Identity.GetUserId());
                    Console.WriteLine(u.FirstName);
                    // Generate a unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Define the folder path where the file will be saved (create the folder if not exists)
                    var folderPath = Path.Combine(Server.MapPath("~/Pictures"));
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Define the full file path
                    var filePath = Path.Combine(folderPath, fileName);

                    // Save the file to the server
                    file.SaveAs(filePath);

                    // Update the user's profile picture path in the database
                    u.ProfilePicturePath = $"/Pictures/{fileName}";
                    
                    db.SaveChanges();

                    // Redirect to the user's profile page or another relevant page
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Handle the exception, log, and return an error view
                    return View("Error");
                }
            }

            // If the file is not provided, return to the upload form with an error
            ModelState.AddModelError("File", "Please choose a file to upload.");
            return View("Index");
        }
    }
}