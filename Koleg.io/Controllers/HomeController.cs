using Koleg.io.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Koleg.io.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            // Fetch subjects from the database
            List<Subject> subjectsFromDb = db.Subjects.Include(s=>s.Uploads).ToList();


            // Calculate the overall score for each subject on the client side
            var bestScoredSubjects = subjectsFromDb
                .OrderByDescending(s => s.Uploads
                    .Where(upload => upload.IsApproved)
                    .SelectMany(upload => upload.Reviews)
                    .DefaultIfEmpty()
                    .Average(review => review != null ? review.Rating : 0))
                .Take(3)
                .ToList();

            return View(bestScoredSubjects);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.Name = db.Users.FirstOrDefault().FirstName;
            return View();
        }
        [Authorize]

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}