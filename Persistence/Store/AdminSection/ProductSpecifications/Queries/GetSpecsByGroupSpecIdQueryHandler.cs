using Application.Store.AdminSection.ProductSpecificationService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Queries
{
    public class GetSpecsByGroupSpecIdQueryHandler : IRequestHandler<GetSpecsByGroupSpecIdQuery, List<SpecDto>>
    {
        private readonly DataBaseContext _context;
        public GetSpecsByGroupSpecIdQueryHandler(DataBaseContext context) => _context = context;

        public async Task<List<SpecDto>> Handle(GetSpecsByGroupSpecIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductSpecifications
    .Where(g => g.ProductSpecificationGroupId == request.GroupSpecId)
    .Select(g => new SpecDto
    {
        Id = g.Id,
        Key = g.Key,
        Value = g.Value
    }).ToListAsync(cancellationToken);

        }
    }
}
