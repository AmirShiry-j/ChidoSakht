using System.ComponentModel.DataAnnotations;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;

namespace WebApi.Areas.Admin.ModelsAndDtoes.RelatedProduct
{
    public class AddRelatedProductsApiDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [MinListCount(1)]
        public List<int> RelatedProductIds { get; set; }
    }

    public class RemoveRelatedProductApiDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [MinListCount(1)]
        public List<int> RelatedProductIds { get; set; }
    }
}
