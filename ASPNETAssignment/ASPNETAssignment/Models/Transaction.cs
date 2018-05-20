using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class Transaction
    {

        public int TransactionID { get; set; }
        // ICollection<KeyValuePair<Tuple<int, int>, int>> ListOfItems { get; } = new List<KeyValuePair<Tuple<int, int>, int>>();
        public string ItemsInTransaction { get; set; }
        public string UserEmail { get; set; }
        public decimal Total { get; set; }
        public string Time { get; set; }


    }
}
