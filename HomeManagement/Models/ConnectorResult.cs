﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Models
{
    public class ConnectorResult
    {
        public bool Success { get; set; }
        public string Exception { get; set; }
        public int LastId { get; set; }
    }
}
