using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models.ViewModels
{
    public class CartErrorViewModel
    {
        public StoreInventory StoreInventory { get; set; }
        public Product Product { get; set; }

        public CartErrorViewModel()
        {

        }

        public CartErrorViewModel(StoreInventory s, Product p)
        {
            StoreInventory = s;
            Product = p;
        }
    }
}
