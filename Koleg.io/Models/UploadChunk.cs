using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Koleg.io.Models
{
    public class UploadChunk
    {
        [Key]
        public int Id { get; set; } // Primary key
        public byte[] ChunkData { get; set; } // Store the data for this chunk

        // Foreign key to link the chunk to the corresponding file
        public int FileId { get; set; }
        public virtual Upload File { get; set; } // Navigation property to represent the associated file
    }

}