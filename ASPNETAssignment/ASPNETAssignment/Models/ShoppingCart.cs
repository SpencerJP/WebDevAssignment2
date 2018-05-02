using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Item> Item { get; } = new List<Item>();
    }
}
