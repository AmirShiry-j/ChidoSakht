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

        public string? Name { get; set; }
    }
}
