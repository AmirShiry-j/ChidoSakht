using Application.Store.AdminSection.ProductSpecificationService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Queries
{
    public class GetSpecGroupByIdQueryHandler : IRequestHandler<GetSpecGroupByIdQuery, ProductSpecificationGroup>
    {
        private readonly DataBaseContext _context;
        public GetSpecGroupByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductSpecificationGroup> Handle(GetSpecGroupByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productSpecificationGroup = await _context.ProductSpecificationGroups.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Retrun It
            return productSpecificationGroup;
        }
    }
}
