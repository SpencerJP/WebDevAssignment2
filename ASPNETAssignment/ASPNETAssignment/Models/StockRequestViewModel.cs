using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class StockRequestViewModel
    {
        public StockRequest sr { get; set; }
        public OwnerInventory oi { get; set; }

        public StockRequestViewModel(StockRequest p1, OwnerInventory p2)
        {
            this.sr = p1;
            this.oi = p2;
        }

    }
}
