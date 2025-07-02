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
