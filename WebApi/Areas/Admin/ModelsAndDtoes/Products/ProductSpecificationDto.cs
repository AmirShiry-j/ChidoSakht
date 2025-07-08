using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class CreateProductSpecificationGroupApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Titile { get; set; }
    }
    public class UpdateProductSpecificationGroupApiDto
    {
        [Required]
        public long ProductSpecificationGroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Titile { get; set; }
    }

    ////
    public class CreateProductSpecificationApiDto
    {
        [Required]
        public long ProductSpecificationGroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Key { get; set; }

        [Required]
        [MaxLength(200)]
        public string Value { get; set; }
    }
    public class UpdateProductSpecificationApiDto
    {
        [Required]
        public long ProductSpecificationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Key { get; set; }

        [Required]
        [MaxLength(200)]
        public string Value { get; set; }
    }
}
