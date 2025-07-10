using Application.Store.AdminSection.ProductSpecificationService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Queries
{
    public class GetSpecGroupsWithProductIdQueryHandler : IRequestHandler<GetSpecGroupsWithProductIdQuery, List<SpecGroupDto>>
    {
        private readonly DataBaseContext _context;
        public GetSpecGroupsWithProductIdQueryHandler(DataBaseContext context) => _context = context;

        public async Task<List<SpecGroupDto>> Handle(GetSpecGroupsWithProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductSpecificationGroups
    .Where(g => g.ProductId == request.ProductId)
    .Select(g => new SpecGroupDto
    {
        Id = g.Id,
        Title = g.Title
    }).ToListAsync(cancellationToken);
        }
    }
}
