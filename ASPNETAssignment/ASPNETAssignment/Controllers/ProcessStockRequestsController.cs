using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNETAssignment.Data;
using Microsoft.AspNetCore.Authorization;
using ASPNETAssignment.Models;

namespace ASPNETAssignment.Controllers
{
    [Authorize(Roles = "Owner")]
    public class ProcessStockRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessStockRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StockRequests
        public async Task<IActionResult> Index()
        {
            var aSPNetAssignmentContext = (from stockRequests in _context.StockRequest.Include(s => s.Product).Include(s => s.Store)
                                           join ownerInventory in _context.OwnerInventory
                                           on stockRequests.ProductID equals ownerInventory.ProductID
                                           select new StockRequestViewModel(stockRequests, ownerInventory));
            return View(await aSPNetAssignmentContext.ToListAsync());
        }

        public async Task<IActionResult> ProcessStockRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stockRequest = await _context.StockRequest
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            var ownerInventory = await _context.OwnerInventory
                .Include(s => s.Product)
                .SingleOrDefaultAsync(m => m.ProductID == stockRequest.ProductID);
            if (stockRequest == null)
            {
                return NotFound();
            }
            if (stockRequest.Quantity <= ownerInventory.StockLevel)
            {
            }
            return View(stockRequest);
        }

        // POST: StockRequests/Delete/5
        [HttpPost, ActionName("ProcessStockRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessStockRequestConfirmed(int id)
        {
            var stockRequest = await _context.StockRequest.SingleOrDefaultAsync(m => m.StockRequestID == id);
            var ownerInventory = await _context.OwnerInventory
                .Include(s => s.Product)
                .SingleOrDefaultAsync(m => m.ProductID == stockRequest.ProductID);
            var storeStock = await _context.StoreInventory
                .SingleOrDefaultAsync(m => (m.StoreID == stockRequest.StoreID && m.ProductID == stockRequest.ProductID));
            if (stockRequest.Quantity <= ownerInventory.StockLevel)
            {
                ownerInventory.StockLevel = ownerInventory.StockLevel - stockRequest.Quantity; // remove quantity from owner's stocklevel
                storeStock.StockLevel = storeStock.StockLevel + stockRequest.Quantity; // give that quantity to the store
                _context.StockRequest.Remove(stockRequest); // delete stock request
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // return view showing that this request cannot work cos stock is too low
                return View("ProcessError", new ProcessStockRequestErrorViewModel(stockRequest));
            }
        }

        private bool StockRequestExists(int id)
        {
            return _context.StockRequest.Any(e => e.StockRequestID == id);
        }
    }
}
