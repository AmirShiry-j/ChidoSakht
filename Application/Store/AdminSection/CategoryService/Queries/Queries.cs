using Domain.Categories;
using MediatR;

namespace Application.Store.AdminSection.CategoryService.Queries
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
        public int? CategoryId { get; set; } = null;
        public GetAllCategoriesAsTreeQuery(int? CategoryId = null)
        {
            this.CategoryId = CategoryId;
        }
    }
    public class GetAllCategoriesQuery : IRequest<List<BriefCategorySampleDto>>
    {
        public GetAllCategoriesQuery()
        {
        }
    }
}
