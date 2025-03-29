using Domain.Categories;
using MediatR;

namespace Application.CategoryService.Queries
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public int Id { get; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetCategoryDetailsByIdQuery : IRequest<CategoryDetailsDto>
    {
        public int Id { get; }

        public GetCategoryDetailsByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class CheckCategoryHasAnyChildrenByIdQuery : IRequest<bool>
    {
        public int Id { get; }

        public CheckCategoryHasAnyChildrenByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetAllCategoriesAsTreeQuery : IRequest<List<BriefCategoryDto>>
    {
        public GetAllCategoriesAsTreeQuery()
        {
        }
    }
}
