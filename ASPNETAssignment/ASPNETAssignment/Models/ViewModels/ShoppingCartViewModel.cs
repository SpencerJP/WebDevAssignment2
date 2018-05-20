using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class ShoppingCartViewModel
    {
        public IDictionary<StoreInventory, int> ListOfItems = new Dictionary<StoreInventory, int>();

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (KeyValuePair<StoreInventory, int> item in ListOfItems)
            {
                total = total + (item.Key.Product.Price * item.Value);  
            }
            return total;
        }

        // for adding shipping
        public decimal GetTotal(decimal additionalcosts)
        {
            decimal total = 0;
            foreach (KeyValuePair<StoreInventory, int> item in ListOfItems)
            {
                total = total + (item.Key.Product.Price * item.Value);
            }
            return total + additionalcosts;
        }
    }
}
