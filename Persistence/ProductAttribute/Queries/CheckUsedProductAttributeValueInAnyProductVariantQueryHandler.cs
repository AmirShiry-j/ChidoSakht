using Application.Store.AdminSection.ProductAttribute.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductAttribute.Queries
{
    public class CheckUsedProductAttributeValueInAnyProductVariantQueryHandler : IRequestHandler<CheckUsedProductAttributeValueInAnyProductVariantQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckUsedProductAttributeValueInAnyProductVariantQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckUsedProductAttributeValueInAnyProductVariantQuery request, CancellationToken cancellationToken)
        {
            var used = await _context.ProductVariantAttributeValues
                .AnyAsync(p => p.ProductAttributeValueId.Equals(request.ProductAttributeValueId));

            return used;
        }
    }
}
