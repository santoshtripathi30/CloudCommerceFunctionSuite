using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCommerceFunctionSuite.Entities
{
    public static class OrderHelper
    {
        public static Order GetExampleOrder() => new Order
        {
            OrderId = 12345,
            CustomerName = "Santosh",
            TotalAmount = 50000,
            Product = "Laptop"
        };
    }
}
