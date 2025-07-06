namespace WebApi.Areas.Admin.ModelsAndDtoes.RelatedProduct
{
    public class AddRelatedProductsApiDto
    {
        public int ProductId { get; set; }
        public List<int> RelatedProductIds { get; set; }
    }

    public class RemoveRelatedProductApiDto
    {
        public int ProductId { get; set; }
        public int RelatedProductId { get; set; }
    }
}
