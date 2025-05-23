using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class SetDescriptionApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
    }
}
