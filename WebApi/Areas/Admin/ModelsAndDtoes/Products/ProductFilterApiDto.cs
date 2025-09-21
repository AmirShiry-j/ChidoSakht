namespace WebApi.Areas.Admin.ModelsAndDtoes.Products
{
    public class ProductFilterApiDto
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
}
