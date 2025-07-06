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
    public class GetExistingRelationsQueryHandler
        : IRequestHandler<GetExistingRelationsQuery, List<Domain.Products.RelatedProduct>>
    {
        private readonly DataBaseContext _context;
        public GetExistingRelationsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Products.RelatedProduct>> Handle(GetExistingRelationsQuery request, CancellationToken cancellationToken)
        {
            var productId = request.ProductId;
            var relatedIds = request.RelatedIds;

            return await _context.Set<Domain.Products.RelatedProduct>()
                .Where(r =>
                    (r.ProductId == productId && relatedIds.Contains(r.RelatedProductId)) ||
                    (r.RelatedProductId == productId && relatedIds.Contains(r.ProductId)))
                .ToListAsync(cancellationToken);
        }
    }

}
