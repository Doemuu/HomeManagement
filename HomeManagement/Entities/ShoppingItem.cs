using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Entities
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Section { get; set; }
        public int Amount { get; set; }
        public int Priority { get; set; }
        public bool IsFavourite { get; set; }
        public bool IsDeleted { get; set; }
    }
}
