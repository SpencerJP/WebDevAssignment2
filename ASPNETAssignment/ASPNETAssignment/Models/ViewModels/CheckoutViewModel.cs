using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class CheckoutViewModel
    {

        [Display(Name = "Credit Card Type")]
        [Required]
        public CardType CreditCardType { get; set; }

        [Display(Name = "Credit Card Number")]
        [Required]
        [CreditCard]
        public string CreditCardNumber { get; set; }

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public CheckoutViewModel(ShoppingCartViewModel shoppingCartViewModel)
        {
            ShoppingCartViewModel = shoppingCartViewModel;
            CreditCardType = CardType.MasterCard; // default value
        }

        public CheckoutViewModel()
        {
        }
    }

}
