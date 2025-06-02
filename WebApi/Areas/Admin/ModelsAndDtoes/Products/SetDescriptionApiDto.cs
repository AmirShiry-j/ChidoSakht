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
}
