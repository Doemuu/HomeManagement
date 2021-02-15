using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Models
{
    public class FileResult
    {
        public bool Success { get; set; }
        public string Exception { get; set; }
        public string SubPath { get; set; }
        public string ContentType { get; set; }
    }
}
