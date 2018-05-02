using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    // class for remembering which store customer is buying product from along with quantity of purchase. 
    public class Item
    {
        public int ItemID { get; set; }

        public StoreInventory StoreInventory { get; set; }

        public int Quantity { get; set; }

    }
}
