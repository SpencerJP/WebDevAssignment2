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

        // GET: StockRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

            return View(stockRequest);
        }

        // GET: StockRequests/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID");
            return View();
        }

        // POST: StockRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockRequestID,StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }

        // GET: StockRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequest.SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }



        // POST: StockRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockRequestID,StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }

        // GET: StockRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

            return View(stockRequest);
        }
        // GET: StockRequests/Delete/5
        public async Task<IActionResult> ProcessStockRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["CanProcess"] = "You cannot process this request, not enough stock in the owner inventory.";
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
                ViewData["CanProcess"] = "Process a stock request.";
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
                return NotFound(); // remove this, for now just leave it but return a proper error message
            }
        }

        // POST: StockRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockRequest = await _context.StockRequest.SingleOrDefaultAsync(m => m.StockRequestID == id);
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
