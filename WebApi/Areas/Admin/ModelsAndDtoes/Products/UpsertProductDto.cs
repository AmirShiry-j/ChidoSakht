using Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class CreateProductApiDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [Range(1, 2)]
        public ProductTypeApi ProductType { get; set; }
    }

    public enum ProductTypeApi
    {
        Sample = 1,
        Variable = 2
    }

    public class UpdateInfoProductSampleApiDto
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

    public class SpecialPriceValidationAttribute : ValidationAttribute
    {
        private readonly string _pricePropertyName;

        public SpecialPriceValidationAttribute(string pricePropertyName)
        {
            _pricePropertyName = pricePropertyName;
            ErrorMessage = "SpecialPrice نباید برابر یا بیشتر از Price باشد.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var specialPrice = value as long?;

            var property = validationContext.ObjectType.GetProperty(_pricePropertyName);
            if (property == null)
                return new ValidationResult($"Property '{_pricePropertyName}' not found.");

            var priceValue = property.GetValue(validationContext.ObjectInstance);
            if (priceValue is long price && specialPrice.HasValue)
            {
                if (specialPrice.Value >= price)
                    return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
