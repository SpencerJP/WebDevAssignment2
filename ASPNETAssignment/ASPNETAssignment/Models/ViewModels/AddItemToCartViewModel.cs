using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class AddItemToCartViewModel
    {
        public AddItemToCartViewModel(int storeID, int productID, int v)
        {
            StoreID = storeID;
            ProductID = productID;
            this.Quantity = v;
        }
        public AddItemToCartViewModel()
        {
        }

        [Range(1, int.MaxValue, ErrorMessage = "Can't be negative or 0!")]
        public int Quantity { get; set; }
        public int StoreID { get; set; }
        public int ProductID { get; set; }
    }
}
