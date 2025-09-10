using Domain.Carts;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Cart
{
    [ExactlyOneRequired(nameof(ProductId), nameof(ProductVariantId))]
    public class AddItemToCartApiDto
    {
        public int? ProductId { get; set; }
        public int? ProductVariantId { get; set; }
    }

    public class DeleteItemInCartApiDto
    {
        [Required]
        public long CartItemId { get; set; }
    }

    [ExactlyOneRequired(nameof(Quantity), nameof(Behavior))]
    public class UpdateQuantityOfItemInCartApiDto
    {
        [Required]
        public long CartItemId { get; set; }
        public int? Quantity { get; set; }
        public Behavior? Behavior { get; set; }
    }

    public class MoveAnItemInCartToAnotherCartApiDto
    {
        [Required]
        public long CartItemId { get; set; }
        [Range(1, 2)]
        [Required]
        public CartType MoveToCartType { get; set; }
    }
    public class GetCartFilterApiDto
    {
        [Range(1, 2)]
        [Required]
        public CartType CartType { get; set; }
    }

    //Validation
    public class ExactlyOneRequiredAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public ExactlyOneRequiredAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = _propertyNames
                .Select(p => validationContext.ObjectType.GetProperty(p))
                .ToList();

            if (properties.Any(p => p == null))
                return new ValidationResult("Property not found.");

            int count = properties
                .Select(p => p.GetValue(validationContext.ObjectInstance))
                .Count(v => v != null);

            if (count == 1)
                return ValidationResult.Success;

            return new ValidationResult($"دقیقاً یکی از فیلدهای ({string.Join(", ", _propertyNames)}) باید مقدار داشته باشد.");
        }
    }
}