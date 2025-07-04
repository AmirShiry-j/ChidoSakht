using Application.Store.AdminSection.CategoryService.Queries;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Categories.Queries
{
    public class GetCategoryDetailsByIdQueryHandler : IRequestHandler<GetCategoryDetailsByIdQuery, CategoryDetailsDto>
    {
        private readonly DataBaseContext _context;

        public GetCategoryDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<CategoryDetailsDto> Handle(GetCategoryDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            //Get Category with all children (subCategories)
            var categoryWithChildren = await GetCategoryWithChildrenExplicitAsync(request.Id, _context);

            return categoryWithChildren;
        }

        public async Task<CategoryDetailsDto> GetCategoryWithChildrenExplicitAsync(int categoryId, DataBaseContext context)
        {
            //Get Category and include parent
            var category = await context.Set<Category>()
                .Include(p => p.ParentCategory)
           .FirstOrDefaultAsync(c => c.Id == categoryId);

            //Check exist
            if (category == null)
                return null;

            //Load all chilren
            await LoadChildrenExplicitlyAsync(category, context);

            //Map and return
            return MapToCategoryDetailsDto(category);
        }

        //Map Category to CategoryDetails
        private CategoryDetailsDto MapToCategoryDetailsDto(Category category)
        {

            return new CategoryDetailsDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                CreateTime = category.CreateTime,
                LastUpdateTime = category.LastUpdateTime,
                ChildCategories = category.ChildCategories.Select(MapToChildCategoryDto).ToList(),
            };
        }

        //Map ChildCategories to ChildCategoryDto
        private ChildCategoryDto MapToChildCategoryDto(Category category)
        {
            return new ChildCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ChildCategories = category.ChildCategories.Select(MapToChildCategoryDto).ToList()
            };
        }

        //Load all chilren by Explicit loading
        private async Task LoadChildrenExplicitlyAsync(Category category, DataBaseContext context)
        {
            await context.Entry(category).Collection(c => c.ChildCategories).LoadAsync();

            foreach (var child in category.ChildCategories)
            {
                await LoadChildrenExplicitlyAsync(child, context);
            }
        }
    }
}
