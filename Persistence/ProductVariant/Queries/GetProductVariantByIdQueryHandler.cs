using Application.ProductVariant.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductVariant.Queries
{
    public class GetProductVariantByIdQueryHandler : IRequestHandler<GetProductVariantByIdQuery, Domain.Products.ProductVariant>
    {
        private readonly DataBaseContext _context;
        public GetProductVariantByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Domain.Products.ProductVariant> Handle(GetProductVariantByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productVariant = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id.Equals(request.ProductVariantId));

            //Retrun It
            return productVariant;
        }
    }
}
