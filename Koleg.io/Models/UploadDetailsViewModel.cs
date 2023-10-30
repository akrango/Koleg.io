using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Koleg.io.Models
{
    public class UploadDetailsViewModel
    {
        public Upload Upload { get; set; } // The upload information
        public Comment Comment { get; set; } // Properties related to adding comments
    }

}