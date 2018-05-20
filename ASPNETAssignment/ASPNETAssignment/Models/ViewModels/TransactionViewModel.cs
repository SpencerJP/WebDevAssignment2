using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class TransactionViewModel
    {
        public string UserID { get; set; }

        public TransactionViewModel()
        {

        }

        public TransactionViewModel(string s)
        {
            UserID = s;
        }
    }
}
