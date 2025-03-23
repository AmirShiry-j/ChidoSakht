namespace WebApi.ModelsAndDtoes.Categories
{
    public class CreateCategoryApiDto
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
