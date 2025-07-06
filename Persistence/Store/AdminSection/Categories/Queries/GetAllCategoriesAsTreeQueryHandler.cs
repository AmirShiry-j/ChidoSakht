using Application.Store.AdminSection.CategoryService.Queries;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Categories.Queries
{
    public class GetAllCategoriesAsTreeQueryHandler : IRequestHandler<GetAllCategoriesAsTreeQuery, List<BriefCategoryDto>>
    {
        private readonly DataBaseContext _context;
        public GetAllCategoriesAsTreeQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<BriefCategoryDto>> Handle(GetAllCategoriesAsTreeQuery request, CancellationToken cancellationToken)
        {
            //Get Categories to 
            var allcategoriesAsTree = await GetAllCategoriesAsTreeExplicitAsync(_context);

            return allcategoriesAsTree;
        }

        public async Task<List<BriefCategoryDto>> GetAllCategoriesAsTreeExplicitAsync(DataBaseContext context)
        {
            //Get main Categories
            var baseCategories = await context.Set<Category>()
                .Where(p => p.ParentCategoryId == null)
                .ToListAsync();


            //Check exist
            if (baseCategories == null || !baseCategories.Any())
                return null;

            //Load all chilren
            foreach (var child in baseCategories)
            {
                await LoadChildrenExplicitlyAsync(child, context);
            }

            //Map category to briefCategoryDto
            var briefCategories = new List<BriefCategoryDto>();
            foreach (var child in baseCategories)
            {
                briefCategories.Add(MapToChildCategoryDto(child));
            }

            //Map and return
            return briefCategories;
        }

        //Map Category to BriefCategoryDto
        private BriefCategoryDto MapToChildCategoryDto(Category category)
        {
            return new BriefCategoryDto
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
