using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class StockRequestViewModel
    {
        public StockRequest StockRequest { get; set; }
        public OwnerInventory OwnerInventory { get; set; }

        public StockRequestViewModel(StockRequest p1, OwnerInventory p2)
        {
            this.StockRequest = p1;
            this.OwnerInventory = p2;
        }

    }
}
