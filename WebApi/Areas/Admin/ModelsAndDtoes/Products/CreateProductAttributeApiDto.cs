using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class CreateProductAttributeApiDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
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

}
