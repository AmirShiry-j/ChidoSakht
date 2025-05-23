using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class UpsertProductApiDto
    {
        public int? ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
