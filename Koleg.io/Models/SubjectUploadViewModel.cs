using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Koleg.io.Models
{
    public class SubjectUploadViewModel
    {
        public int SubjectId { get; set; }
        public int UploadId { get; set; }
        public Subject Subject { get; set; }
        public Upload Upload{ get; set; }
    }
}