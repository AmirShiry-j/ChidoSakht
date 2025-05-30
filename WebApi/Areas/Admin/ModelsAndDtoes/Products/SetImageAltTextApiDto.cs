using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class SetImageAltTextApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(100)]
        public string? ImageAltText { get; set; }
    }

    public class SetIndexImageApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class CreateProductAttributeApiDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public AttributeTypeApi AttributeType { get; set; }
    }

    public enum AttributeTypeApi
    {
        Selective=1,
        Colored=2
    }


}
