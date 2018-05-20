using ASPNETAssignment.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class ShoppingCart
    {
        public ICollection<KeyValuePair<Tuple<int, int>, int>> ListOfItems { get; } = new List<KeyValuePair<Tuple<int, int>, int>>();

        public async Task<ReadableItemList> MakeReadable(ApplicationDbContext context)
        {
            var ril = new ReadableItemList();
            foreach (KeyValuePair<Tuple<int, int>, int> i in ListOfItems)
            {
               var storeInventory = await context.StoreInventory
               .Include(s => s.Product)
               .Include(s => s.Store)
               .SingleOrDefaultAsync(m => m.StoreID == i.Key.Item2 && m.ProductID == i.Key.Item1);
                    ril.ListOfItems.Add(new KeyValuePair<Tuple<string, string>, int>(new Tuple<string, string>(storeInventory.Store.Name, storeInventory.Product.Name), i.Value));
            };
            return ril;
        }
    }
}
