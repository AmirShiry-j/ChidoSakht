using Application.Store.UserSection.ProductVariantService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.ProductVariant.Queries
{
    public class GetVariantByIdInUserSectionQueryHandler : IRequestHandler<GetProductVariantByInUserSectionIdQuery, Domain.Products.ProductVariant>
    {
        private readonly DataBaseContext _context;
        public GetVariantByIdInUserSectionQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Domain.Products.ProductVariant> Handle(GetProductVariantByInUserSectionIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productVariant = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id.Equals(request.ProductVariantId) );

            //Retrun It
            return productVariant;
        }
    }
}
