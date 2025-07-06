using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public class ProductSpecificationGroup
    {
        public long Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Title { get; set; }

        public ICollection<ProductSpecification> Specifications { get; set; }
    }
    public class ProductSpecification
    {
        public long Id { get; set; }

        public long ProductSpecificationGroupId { get; set; }
        public ProductSpecificationGroup ProductSpecificationGroup { get; set; }

        public string Key { get; set; }   // مثلاً "رنگ"
        public string Value { get; set; } // مثلاً "قرمز"
    }
}
