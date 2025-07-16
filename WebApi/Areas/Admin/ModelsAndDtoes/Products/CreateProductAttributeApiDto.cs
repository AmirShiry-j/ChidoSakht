using System.ComponentModel.DataAnnotations;
using System.Collections;

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
        [Required]
        public bool UseForVariant { get; set; }
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
        [Required]
        public bool UseForVariant { get; set; }
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
        [Range(0, long.MaxValue)]
        public long Price { get; set; }
        [Range(0, long.MaxValue)]
        [SpecialPriceValidation(nameof(Price))]
        public long? SpecialPrice { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        [Required]
        [MinListCount(1)]
        public List<int> ProductAttributeValueIds { get; set; }

        //
        [Range(0, double.MaxValue)]
        public double? Length { get; set; }
        [Range(0, double.MaxValue)]
        public double? Width { get; set; }
        [Range(0, double.MaxValue)]
        public double? Height { get; set; }
        [Range(0, double.MaxValue)]
        public double? Weight { get; set; }
    }



    public class MinListCountAttribute : ValidationAttribute
    {
        private readonly int _minCount;

        public MinListCountAttribute(int minCount)
        {
            _minCount = minCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as ICollection;
            if (list is null)
                return ValidationResult.Success;


            if (list.Count < _minCount)
            {
                return new ValidationResult($"The list must contain at least {_minCount} item(s).");
            }
            return ValidationResult.Success;
        }
    }

    public class UpdateProductVariantApiDto
    {
        [Required]
        public int ProductVariantId { get; set; }
        [Required]
        [Range(0, long.MaxValue)]
        public long Price { get; set; }
        [Range(0, long.MaxValue)]
        [SpecialPriceValidation(nameof(Price))]
        public long? SpecialPrice { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        //
        [Range(0, double.MaxValue)]
        public double? Length { get; set; }
        [Range(0, double.MaxValue)]
        public double? Width { get; set; }
        [Range(0, double.MaxValue)]
        public double? Height { get; set; }
        [Range(0, double.MaxValue)]
        public double? Weight { get; set; }
    }


   

}
