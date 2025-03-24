using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Categories
{
    public class UpdateCategoryApiDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
