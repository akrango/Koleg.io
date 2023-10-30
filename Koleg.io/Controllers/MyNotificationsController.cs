using Koleg.io.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Koleg.io.Controllers
{
    public class MyNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: MyNotifications
        public int GetNotificationCount()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            int count = user.MyUploads.Where(up => up.IsCommentedOn == true).Count();
            return count;
        }
    }
}