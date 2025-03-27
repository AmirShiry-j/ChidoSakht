using Domain.Categories;
using MediatR;

namespace Application.CategoryService.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; }
        public int? ParentCategoryId { get; }

        public CreateCategoryCommand(string Name, int? ParentCategoryId)
        {
            this.Name = Name;
            this.ParentCategoryId = ParentCategoryId;
        }
    }
    public class DeleteCategoryCommand : IRequest
    {
        public Category Category { get; set; }
        public DeleteCategoryCommand(Category category)
        {
            Category = category;
        }
    }
    public class UpdateCategoryCommand : IRequest
    {
        public Category Category { get; set; }
        public UpdateCategoryCommand(Category category)
        {
            Category = category;
        }
    }
}
