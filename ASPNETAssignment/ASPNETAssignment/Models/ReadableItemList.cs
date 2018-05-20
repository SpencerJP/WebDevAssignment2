using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class ReadableItemList
    {
        // changes the tuple from storing ints to storing strings to make it readable on the angular side
        public ICollection<KeyValuePair<Tuple<string, string>, int>> ListOfItems { get; } = new List<KeyValuePair<Tuple<string, string>, int>>();
    }
}
