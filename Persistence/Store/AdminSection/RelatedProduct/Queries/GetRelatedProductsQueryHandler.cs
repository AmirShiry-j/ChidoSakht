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
    public class GetRelatedProductsQueryHandler : IRequestHandler<GetRelatedProductsQuery, List<ProductDto>>
    {
        private readonly DataBaseContext _context;
        public GetRelatedProductsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetRelatedProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Set<Domain.Products.RelatedProduct>().IgnoreQueryFilters()
                .Where(r => r.ProductId == request.ProductId)
                .Include(r => r.RelatedTo)
                .Select(r => new ProductDto
                {
                    ProductId = r.RelatedTo.Id,
                    ProductName = r.RelatedTo.Name
                })
                .ToListAsync(cancellationToken);
        }
    }

}
