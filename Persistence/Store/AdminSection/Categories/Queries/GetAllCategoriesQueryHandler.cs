using Application.Store.AdminSection.CategoryService.Queries;
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
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<BriefCategorySampleDto>>
    {
        private readonly DataBaseContext _context;
        public GetAllCategoriesQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<BriefCategorySampleDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories.IgnoreQueryFilters().Select(p => new BriefCategorySampleDto
            {
                Id = p.Id,
                Name = p.Name
            })
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }
    }
}
