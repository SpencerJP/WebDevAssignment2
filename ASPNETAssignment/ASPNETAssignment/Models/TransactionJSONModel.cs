using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    // the purpose of this class is to convert the string version of ItemsInTransaction into a ICollection etc so that it can be reserialized for the api, to make it readable to angular
    public class TransactionJSONModel
    {
        // ICollection<KeyValuePair<Tuple<int, int>, int>> ListOfItems { get; } = new List<KeyValuePair<Tuple<int, int>, int>>();
        public ICollection<KeyValuePair<Tuple<string, string>, int>> ItemsInTransaction { get; set; }
        public string UserEmail { get; set; }
        public decimal Total { get; set; }
        public string Time { get; set; }
    }
}
