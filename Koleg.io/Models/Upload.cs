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
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<UploadChunk> Chunks { get; set; }
        public bool IsCommentedOn { get; set; }
        public Upload() {
            Comments=new List<Comment>();
            Chunks=new List<UploadChunk>();
            IsCommentedOn = false;
        }
    }
}