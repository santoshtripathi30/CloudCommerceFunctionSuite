using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCommerceFunctionSuite.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public double TotalAmount { get; set; }
        public string Product { get; set; }
    }
}
