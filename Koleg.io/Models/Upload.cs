using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Koleg.io.Models;

namespace Koleg.io.Models
{
    public class Upload
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="File Name")]
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public virtual List<Review> Reviews { get; set; }

        public bool IsCommentedOn { get; set; }
        public bool IsApproved { get; set; }
        public int NumberOfRatingVotes { get; set; }
        public int TotalSumOfRatings { get; set; }
        public Upload() {
            IsCommentedOn = false;
            IsApproved = false;
            Reviews = new List<Review>();
        }
    }
}