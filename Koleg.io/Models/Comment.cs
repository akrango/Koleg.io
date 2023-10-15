using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Koleg.io.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int UploadId { get; set; } // ID of the associated uploaded file
        public string UserId { get; set; } // ID of the logged in user
        public string CommentText { get; set; }
        public virtual ApplicationUser User{ get; set; }
    }

}