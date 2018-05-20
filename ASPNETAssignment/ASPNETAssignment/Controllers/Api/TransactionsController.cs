using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETAssignment.Data;
using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ASPNETAssignment.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<TransactionJSONModel> GetTransaction()
        {
            List<TransactionJSONModel> toReturn = new List<TransactionJSONModel>(); // all just to convert the string of Transaction to the Keyvaluepair of TransactionJSONModel
            foreach (Transaction t in _context.Transaction)
            {
                var newTransaction = new TransactionJSONModel
                {
                    ItemsInTransaction = JsonConvert.DeserializeObject<List<KeyValuePair<Tuple<string, string>, int>>>(t.ItemsInTransaction),
                    UserEmail = t.UserEmail,
                    Total = t.Total,
                    Time = t.Time
                };
                toReturn.Add(newTransaction);
            }
            return toReturn;
        }

        // GET: api/Transactions/5
        [HttpGet("{userid}")]
        public async Task<IActionResult> GetTransactionAsync([FromRoute] string userid)
        {
            System.Diagnostics.Debug.WriteLine(userid);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userid);
            var userEmail = user.Email;
            var transactions = (from a in _context.Transaction
                               where (a.UserEmail == userEmail)
                               select a);
            if (transactions == null)
            {
                return NotFound();
            }

            List<TransactionJSONModel> toReturn = new List<TransactionJSONModel>(); // all just to convert the string of Transaction to the Keyvaluepair of TransactionJSONModel
            foreach (Transaction t in transactions)
            {
                var newTransaction = new TransactionJSONModel
                {
                    ItemsInTransaction = JsonConvert.DeserializeObject<List<KeyValuePair<Tuple<string, string>, int>>>(t.ItemsInTransaction),
                    UserEmail = t.UserEmail,
                    Total = t.Total,
                    Time = t.Time
                };
                toReturn.Add(newTransaction);
            }
            return Ok(toReturn);
        }
    }
}