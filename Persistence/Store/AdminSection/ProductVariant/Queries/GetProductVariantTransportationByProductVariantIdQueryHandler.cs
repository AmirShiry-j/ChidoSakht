using Application.Store.AdminSection.ProductVariant.Queries;
using Domain.Products;
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
    public class GetProductVariantTransportationByProductVariantIdQueryHandler : IRequestHandler<GetProductVariantTransportationByProductVariantIdQuery, ProductVariantTransportation>
    {
        private readonly DataBaseContext _context;
        public GetProductVariantTransportationByProductVariantIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductVariantTransportation> Handle(GetProductVariantTransportationByProductVariantIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productVariantTransportation = await _context.ProductVariantTransportations.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.ProductVariantId.Equals(request.ProductVariantId));

            //Retrun It
            return productVariantTransportation;
        }
    }
}
