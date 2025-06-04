using Application.ProductService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Products.Queries
{
    public class CheckUsedProductInAnySefareshQueryHandler : IRequestHandler<CheckUsedProductInAnySefareshQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckUsedProductInAnySefareshQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckUsedProductInAnySefareshQuery request, CancellationToken cancellationToken)
        {
            var variantIds_SampleOrVariable = await _context.ProductVariants
                .Where(p => p.ProductId.Equals(request.ProductId))
                .Select(p => p.Id).ToListAsync();

            if (!variantIds_SampleOrVariable.Any())
                return false;

            var used = await _context.SymbolicOrderOrSymbolicShoppingCartItems
                .AnyAsync(p => variantIds_SampleOrVariable.Contains(p.ProductVariantId));

            return used;
        }
    }
}
