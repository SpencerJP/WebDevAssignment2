using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Must be 0 or more!")]
        public decimal Price { get; set; }
    }
}
