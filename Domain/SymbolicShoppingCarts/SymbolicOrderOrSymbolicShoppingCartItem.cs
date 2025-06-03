using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SymbolicShoppingCarts
{
    public class SymbolicOrderOrSymbolicShoppingCartItem
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public ProductVariant ProductVariant { get; set; }
        public int ProductVariantId { get; set; }
    }
}
