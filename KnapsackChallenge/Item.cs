using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackChallenge
{
    /// <summary>
    /// Simple object to hold value-weight pairs for increased clarity.
    /// </summary>
    class Item
    {
        public int value, weight;
        public Item(int value, int weight)
        {
            this.value = value;
            this.weight = weight;
        }
    }
}
