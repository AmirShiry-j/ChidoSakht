using Application.CategoryService.Queries;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //Find and return
            return await _context.Categories
                .Where(c => c.Id == request.Id)
                .Select(c => new CategoryDetailsDto { Id = c.Id, Name = c.Name, ParentCategoryId = c.ParentCategoryId, ParentCategoryName = c.ParentCategory.Name ?? null })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly DataBaseContext _context;

        public GetCategoryByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            //Find and return
            return await _context.Categories
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
