using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Models
{
    public class NoteCreate
    {
        [Required]
        public string NoteTitle { get; set; }
        public IFormFile NoteFile { get; set; }
    }
}
