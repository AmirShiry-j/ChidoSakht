using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class CreateProductAttributeApiDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(1, 2)]
        public AttributeTypeApi AttributeType { get; set; }
    }
    public enum AttributeTypeApi
    {
        Selective = 1,
        Colored = 2
    }

    public class UpdateProductAttributeApiDto
    {
        [Required]
        public int ProductAttributeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    public class CreateValueForAttributeApiDto
    {
        [Required]
        public int ProductAttributeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Value { get; set; }
    }

    public class UpdateProductAttributeValueApiDto
    {
        [Required]
        public int ProductAttributeValueId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Value { get; set; }
    }

    public class CreateProductVariantApiDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public List<int> ProductAttributeValueIds { get; set; }
    }
}
