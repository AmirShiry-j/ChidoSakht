using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Categories
{
    public class CreateCategoryApiDto
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
