using Application.Store.AdminSection.ProductAttribute.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.ProductAttribute.Queries
{
    public class CheckUsedProductAttributeInAnyProductVariantQueryHandler : IRequestHandler<CheckUsedProductAttributeInAnyProductVariantQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckUsedProductAttributeInAnyProductVariantQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckUsedProductAttributeInAnyProductVariantQuery request, CancellationToken cancellationToken)
        {
            var valueIds = await _context.ProductAttributeValues.Where(p => p.ProductAttributeId.Equals(request.ProductAttributeId))
                .Select(p => p.Id)
                .ToListAsync();

            if (!valueIds.Any())
                return false;

            var used = await _context.ProductVariantAttributeValues
                .AnyAsync(p => valueIds.Contains(p.ProductAttributeValueId));

            return used;
        }
    }
}
