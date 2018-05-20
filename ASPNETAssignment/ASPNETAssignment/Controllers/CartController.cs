using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNETAssignment.Data;
using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ASPNETAssignment.Models.ViewModels;

namespace ASPNETAssignment.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private ApplicationUser GetUser()
        {
            return _userManager.GetUserAsync(HttpContext.User).Result;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.Get<ShoppingCart>("cart") != null)
            {
                return View(await ConvertCartToViewModel(HttpContext.Session.Get<ShoppingCart>("cart")));
            }
            else
            {
                return NotFound();
            }
        }

        // GET: CustomerProducts/AddToCart/5
        public async Task<IActionResult> AddToCart(int? storeID, int? productID)
        {
            if (storeID == null || productID == null)
            {
                return NotFound();
            }

            var storeInventory = await _context.StoreInventory.SingleOrDefaultAsync(m => m.StoreID == storeID && m.ProductID == productID);
            if (storeInventory == null)
            {
                return NotFound();
            }
            var vm = new AddItemToCartViewModel((int) storeID, (int) productID, 1);
            return View(vm);
        }

        // POST: CustomerProducts/AddToCart/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([Bind("Quantity,StoreID,ProductID")] AddItemToCartViewModel viewModel)
        {
            ShoppingCart cart;
            if (HttpContext.Session.Get<ShoppingCart>("cart") != null)
            {
                cart = HttpContext.Session.Get<ShoppingCart>("cart");
            }
            else
            {
                cart = new ShoppingCart();
            }
            // add item to cart
            var itemToAdd = new KeyValuePair<Tuple<int, int>, int>(new Tuple<int,int>(viewModel.ProductID, viewModel.StoreID), viewModel.Quantity);
            cart.ListOfItems.Add(itemToAdd);
            HttpContext.Session.Set("cart", cart);
            return RedirectToAction(nameof(CustomerProductsController.Index), "CustomerProducts");


        }

     

        // converts the shoppingCart into a more readable viewmodel
        private async Task<ShoppingCartViewModel> ConvertCartToViewModel(ShoppingCart shoppingCart)
        {
            var vm = new ShoppingCartViewModel();
            foreach (KeyValuePair<Tuple<int, int>, int> i in shoppingCart.ListOfItems)
            {
                try
                {
                    // add to store viewmodel
                    var storeInventory = await _context.StoreInventory
               .Include(s => s.Product)
               .Include(s => s.Store)
               .SingleOrDefaultAsync(m => m.StoreID == i.Key.Item2 && m.ProductID == i.Key.Item1);
                    vm.ListOfItems.Add(storeInventory, i.Value);
                }
                catch(Exception) // catches an exception thrown if key is already used, instead of trying to create identical key it adds to the value of the same key
                {
                    var storeInventory = await _context.StoreInventory
               .Include(s => s.Product)
               .Include(s => s.Store)
               .SingleOrDefaultAsync(m => m.StoreID == i.Key.Item2 && m.ProductID == i.Key.Item1);
                    vm.ListOfItems[storeInventory] = vm.ListOfItems[storeInventory] + i.Value;
                }
            }
            return vm;
        }

        public IActionResult RemoveFromCart(int? storeID, int? productID)
        {
            if (storeID == null || productID == null)
            {
                return NotFound();
            }
            ShoppingCart shoppingCart;
            if (HttpContext.Session.Get<ShoppingCart>("cart") != null)
            {
                shoppingCart = HttpContext.Session.Get<ShoppingCart>("cart");
                
            }
            else
            {
                return NotFound();
            }
            // get item and remove it
            var toRemove = shoppingCart.ListOfItems.SingleOrDefault(r => r.Key.Item2 == storeID && r.Key.Item1 == productID);
            if (shoppingCart.ListOfItems.Remove(toRemove))
            {
                HttpContext.Session.Set("cart", shoppingCart);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<ActionResult> Checkout()
        {

            if (HttpContext.Session.Get<ShoppingCart>("cart") != null)
            {
                // gets the cart items
                return View(new CheckoutViewModel(await ConvertCartToViewModel(HttpContext.Session.Get<ShoppingCart>("cart"))));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(CheckoutViewModel vm)
        {
            CardType expectedCardType = CardTypeInfo.GetCardType(vm.CreditCardNumber);
            if (expectedCardType == CardType.Unknown || expectedCardType != vm.CreditCardType)
            {
                ModelState.AddModelError("CreditCardType", "The Credit Card Type field does not match against the credit card number.");
            }

            if (!ModelState.IsValid)
            {
                if (HttpContext.Session.Get<ShoppingCart>("cart") != null)
                {
                    return View(new CheckoutViewModel(await ConvertCartToViewModel(HttpContext.Session.Get<ShoppingCart>("cart"))));
                }
                else
                {
                    return NotFound();
                }
            }

            //create a new transaction 
            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("cart");

            


            var transactionText = JsonConvert.SerializeObject(shoppingCart.MakeReadable(_context).Result.ListOfItems);
            if (transactionText == null)
            {
                return NotFound();
            }
            // create new transaction to add to db
            var transaction = new Transaction
            {
                ItemsInTransaction = transactionText,
                UserEmail = GetUser().Email,
                Total = ConvertCartToViewModel(shoppingCart).Result.GetTotal() + 15, // 15 is for shipping
                Time = DateTime.Now.ToString()
            };
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Add(transaction);

            // reduce stock level of bought items
            foreach(KeyValuePair<Tuple<int, int>, int> i in shoppingCart.ListOfItems)
            {
                var storeInventory = await _context.StoreInventory
               .Include(s => s.Product)
               .Include(s => s.Store)
               .SingleOrDefaultAsync(m => m.StoreID == i.Key.Item2 && m.ProductID == i.Key.Item1);
                if (storeInventory.StockLevel < i.Value)
                {
                    return View("CartError", new CartErrorViewModel(storeInventory, storeInventory.Product));
                } else
                {

                }
                storeInventory.StockLevel = storeInventory.StockLevel - i.Value;
            }


            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("cart"); // reset cart
            return RedirectToAction(nameof(ViewTransactions));
        }

        public IActionResult ViewTransactions()
        {
            // shows transactions using angular
            return View(new TransactionViewModel(GetUser().Id));
        }
    }
}
