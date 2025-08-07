using Application.Store.AdminSection.ProductVariant.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductVariant.Queries
{
    public class CheckUsedVariantInAnySefareshQueryHandler : IRequestHandler<CheckUsedVariantInAnySefareshQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckUsedVariantInAnySefareshQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckUsedVariantInAnySefareshQuery request, CancellationToken cancellationToken)
        {
            var isUsed = await _context.SymbolicOrderOrSymbolicShoppingCartItems.IgnoreQueryFilters().AnyAsync(p => p.ProductVariantId.Equals(request.ProductVariantId));

            return isUsed;
        }
    }
}
