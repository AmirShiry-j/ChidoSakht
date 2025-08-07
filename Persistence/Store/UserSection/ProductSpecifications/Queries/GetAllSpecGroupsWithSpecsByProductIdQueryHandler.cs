using Application.Store.UserSection.ProductSpecificationService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.ProductSpecifications.Queries
{
    public class GetAllSpecGroupsWithSpecsByProductIdQueryHandler : IRequestHandler<GetAllSpecGroupsWithSpecsByProductIdQuery, List<SpecGroupWithSpecsDto>>
    {
        private readonly DataBaseContext _context;
        public GetAllSpecGroupsWithSpecsByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<SpecGroupWithSpecsDto>> Handle(GetAllSpecGroupsWithSpecsByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductSpecificationGroups
                .Where(g => g.ProductId == request.ProductId)
                .Include(g => g.Specifications)
                .Select(g => new SpecGroupWithSpecsDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    Specifications = g.Specifications.Select(s => new SpecDto
                    {
                        Id = s.Id,
                        Key = s.Key,
                        Value = s.Value
                    }).ToList()
                }).ToListAsync(cancellationToken);
        }
    }
}
