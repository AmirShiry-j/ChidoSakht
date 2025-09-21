using Application.Store.AdminSection.ProductSpecificationService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Queries
{
    public class GetAllSpecGroupsWithSpecsByProductIdQueryHandler : IRequestHandler<GetAllSpecGroupsWithSpecsByProductIdQuery, List<SpecGroupWithSpecsDto>>
    {
        private readonly DataBaseContext _context;
        public GetAllSpecGroupsWithSpecsByProductIdQueryHandler(DataBaseContext context) => _context = context;

        public async Task<List<SpecGroupWithSpecsDto>> Handle(GetAllSpecGroupsWithSpecsByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductSpecificationGroups.IgnoreQueryFilters()
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
