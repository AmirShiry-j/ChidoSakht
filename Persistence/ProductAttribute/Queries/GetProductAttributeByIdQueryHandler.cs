using Application.ProductAttribute.Queries;
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
    public class GetProductAttributeByIdQueryHandler : IRequestHandler<GetProductAttributeByIdQuery, Domain.Products.ProductAttribute>
    {
        private readonly DataBaseContext _context;
        public GetProductAttributeByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Domain.Products.ProductAttribute> Handle(GetProductAttributeByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productAttribute = await _context.ProductAttributes.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Retrun It
            return productAttribute;
        }
    }
}
