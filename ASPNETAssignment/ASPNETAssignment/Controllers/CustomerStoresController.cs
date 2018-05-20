using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNETAssignment.Data;
using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASPNETAssignment.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerStoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerStoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerStores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Store.ToListAsync());
        }
    }
}
