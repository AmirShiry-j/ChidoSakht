using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Nav rel
        public int ProductId { get; set; }
    }
}
