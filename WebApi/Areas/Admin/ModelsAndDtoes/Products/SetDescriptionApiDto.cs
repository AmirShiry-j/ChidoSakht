using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class SetNameProductApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }


    public class SetDescriptionApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }
    }

    public class SetUniqeLinkApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(100)]
        public string? UniqeLink { get; set; }
    }

    public class SetUniCodeApiDto
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(50)]
        public string? UniCode { get; set; }
    }

    public class SetCategoryIdApiDto
    {
        [Required]
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }
    }
}
