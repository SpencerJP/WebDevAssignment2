using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNETAssignment.Data;
using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ASPNETAssignment.Controllers
{
    [Authorize]
    public class StoreInventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public StoreInventoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: StoreInventories
        public async Task<IActionResult> Index()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var applicationDbContext = from a in (_context.StoreInventory.Include(s => s.Product).Include(s => s.Store))
                                       where a.StoreID == currentUser.StoreID select a;

            return View(await applicationDbContext.ToListAsync());
        }

    }
}
