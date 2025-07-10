using Application.Store.AdminSection.RelatedProduct.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.RelatedProduct.Queries
{
    public class GetExistingProductIdsForSetRelatedQueryHandler : IRequestHandler<GetExistingProductIdsForSetRelatedQuery, List<int>>
    {
        private readonly DataBaseContext _context;
        public GetExistingProductIdsForSetRelatedQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<int>> Handle(GetExistingProductIdsForSetRelatedQuery request, CancellationToken cancellationToken)
        {
            var ids = await _context.Products.Where(p => request.RecivedIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            return ids;
        }
    }
}
