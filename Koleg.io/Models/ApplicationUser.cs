using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Koleg.io.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Index { get; set; }
        public List<Upload> MyUploads { get; set; }
        public string ProfilePicturePath { get; set; }

        public ApplicationUser() {
            MyUploads = new List<Upload>();
        }
    }
}