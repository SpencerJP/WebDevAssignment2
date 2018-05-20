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
using Microsoft.AspNetCore.Authorization;

namespace ASPNETAssignment.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerProducts
        public async Task<IActionResult> Index(
            int? storeID,
            string currentFilter,
            string searchString,
            int? page)
        {

            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // current string to search with
            ViewData["CurrentFilter"] = searchString;
            
            var products = from s in _context.StoreInventory.Include(o => o.Product).Include(o => o.Store)
                            select s;
            if (storeID != null)
            {
                // only from a certain store if storeID not null
                products = products.Where(s => s.StoreID == storeID);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                // filter by search string 
                products = products.Where(s => s.Product.Name.Contains(searchString)
                                       || s.Product.Name.Contains(searchString));
            }
            
            // page = 5 seems reasonable for this website
            int pageSize = 5;
            return View(await PaginatedList<StoreInventory> // create a paginated list of items
                .CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
        }

    }
}
