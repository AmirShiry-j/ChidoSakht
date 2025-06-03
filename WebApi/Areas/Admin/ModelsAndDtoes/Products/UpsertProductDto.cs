using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class UpsertProductApiDto
    {
        public int? ProductId { get; set; }
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
}
