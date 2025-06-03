using Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Products
{
    public class Product
    {
        public Product(string name)
        {
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public string? UniCode { get; set; }
        public string? Description { get; set; }
        public string? UniqeLink { get; set; }
        public string? ImageAltText { get; set; }
        public string? NameIndexImage { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public bool IsPublished { get; set; }

        //naves
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
    }

    public enum ProductType
    {
        Sample = 1,
        Variable = 2
    }
    public class ProductAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public AttributeType AttributeType { get; set; }
        public string Name { get; set; }
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }
    }
    public enum AttributeType
    {
        Selective,
        Colored
    }
    public class ProductAttributeValue
    {
        public int Id { get; set; }
        public int ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
        public string Value { get; set; }
    }

    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int Stock { get; set; }
        //
        public ICollection<ProductVariantAttributeValue> ProductVariantAttributeValues { get; set; }
        public ProductVariantTransportation ProductVariantTransportation { get; set; }
    }
    public class ProductVariantTransportation
    {
        public int Id { get; set; }
        //
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        //
        public ProductVariant ProductVariant { get; set; }
        public int ProductVariantId { get; set; }
    }

    public class ProductVariantAttributeValue
    {
        public int Id { get; set; }

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int ProductAttributeValueId { get; set; }
        public ProductAttributeValue ProductAttributeValue { get; set; }
    }
}
