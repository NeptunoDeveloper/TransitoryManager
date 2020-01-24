using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class OrderedItem
    {
        public string ItemName;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public decimal LineTotal;

        // Calculate is a custom method that calculates the price per item
        // and stores the value in a field.
        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }
    }
}
