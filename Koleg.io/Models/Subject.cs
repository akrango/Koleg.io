using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Koleg.io.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Semester { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage ="Subject year must be between 1 and 4")]
        public int Year { get; set; }
        public virtual ICollection<Upload> Uploads { get; set; }
        public Subject()
        {
            Uploads = new List<Upload>();
        }

    }
}