using Application.Store.AdminSection.ProductSpecification.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecification.Queries
{
    public class GetSpecGroupsHandler : IRequestHandler<GetSpecGroupsQuery, List<SpecGroupDto>>
    {
        private readonly DataBaseContext _context;
        public GetSpecGroupsHandler(DataBaseContext context) => _context = context;

        public async Task<List<SpecGroupDto>> Handle(GetSpecGroupsQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductSpecificationGroups
                .Where(g => g.ProductId == request.ProductId)
                .Include(g => g.Specifications)
                .Select(g => new SpecGroupDto
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
