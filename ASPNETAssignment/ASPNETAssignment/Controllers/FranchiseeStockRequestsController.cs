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
using Microsoft.AspNetCore.Identity;

namespace ASPNETAssignment.Controllers
{   
    [Authorize]
    public class FranchiseeStockRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser currentUser;

        public FranchiseeStockRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private ApplicationUser GetUser()
        {
            return _userManager.GetUserAsync(HttpContext.User).Result;
        }

        // GET: FranchiseeStockRequests
        public async Task<IActionResult> Index()
        {
            currentUser = GetUser();
            var stockRequests = (from a in (_context.StockRequest.Include(s => s.Product).Include(s => s.Store))
                                where (a.StoreID == currentUser.StoreID)
                                select a); // only returns this user's store's stock requests
            return View(await stockRequests.ToListAsync());
        }

        // GET: FranchiseeStockRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            currentUser = GetUser();

            var stockRequest = await _context.StockRequest
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }
            if (stockRequest.StoreID != currentUser.StoreID)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // GET: FranchiseeStockRequests/Create
        public IActionResult Create(int? productID)
        {
            if (productID != null)
            {
                ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "Name", 12);
            }
            else
            {
                ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "Name");
            }
            return View();
        }

        // POST: FranchiseeStockRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockRequestID,ProductID,Quantity")] StockRequest stockRequest)
        {
            var storeID = GetUser().StoreID;
            if (storeID != null)
            {
                stockRequest.StoreID = (int) storeID;
            } else
            {
                NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", stockRequest.ProductID);
            return View(stockRequest);
        }

        // GET: FranchiseeStockRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            currentUser = GetUser();
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequest.SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }
            if (stockRequest.StoreID != currentUser.StoreID)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }

        // POST: FranchiseeStockRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockRequestID,StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            currentUser = GetUser();
            if (id != stockRequest.StockRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockRequestExists(stockRequest.StockRequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (stockRequest.StoreID != currentUser.StoreID)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", stockRequest.ProductID);
            //ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }

        // GET: FranchiseeStockRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            currentUser = GetUser();
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequest
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }
            if (stockRequest.StoreID != currentUser.StoreID)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // POST: FranchiseeStockRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            currentUser = GetUser();
            var stockRequest = await _context.StockRequest.SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest.StoreID != currentUser.StoreID)
            {
                return NotFound();
            }
            _context.StockRequest.Remove(stockRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockRequestExists(int id)
        {
            return _context.StockRequest.Any(e => e.StockRequestID == id);
        }
    }
}
