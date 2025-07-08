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
    public class CheckCategoryHasAnyChildrenByIdQueryHandler : IRequestHandler<CheckCategoryHasAnyChildrenByIdQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckCategoryHasAnyChildrenByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckCategoryHasAnyChildrenByIdQuery request, CancellationToken cancellationToken)
        {
            //Find and return
            return await _context.Categories
                .Where(c => c.ParentCategoryId == request.Id)
                .AnyAsync();
        }
    }
}
