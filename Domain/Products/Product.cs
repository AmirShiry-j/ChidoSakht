using Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{

    public class Product
    {
        public int Id { get; set; }
        public string UniCode { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVariable { get; set; }

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    }


    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public long Price { get; set; } 
        public int Stock { get; set; }

        public ICollection<VariantAttributeValue> AttributeValues { get; set; } = new List<VariantAttributeValue>();
    }


    public class ProductAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Name { get; set; }
        public ICollection<ProductAttributeValue> Values { get; set; } = new List<ProductAttributeValue>();
    }

    public class ProductAttributeValue
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public ProductAttribute Attribute { get; set; }

        public string Value { get; set; }
    }

    public class VariantAttributeValue
    {
        public int Id { get; set; }

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int AttributeValueId { get; set; }
        public ProductAttributeValue AttributeValue { get; set; }
    }



}
