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
        public int Id { get; }

        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public UpdateCategoryCommand(int id, string name, int? parentCategoryId)
        {
            Id = id;
            Name = name;
            ParentCategoryId = parentCategoryId;
        }
    }
}
