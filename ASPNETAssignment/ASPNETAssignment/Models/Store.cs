using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDevAssignment2.Models
{
    public class Store
    {
        public int StoreID { get; set; }
        public string Name { get; set; }

        public ICollection<StoreInventory> StoreInventory { get; } = new List<StoreInventory>();
    }
}
