using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public string NoteTitle { get; set; }
        public DateTime UploadDate { get; set; }
        public string Ending { get; set; }
        public bool IsDeleted { get; set; }
    }
}
