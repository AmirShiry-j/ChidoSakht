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
    public class GetProductAttributeDetailsByIdQueryHandler : IRequestHandler<GetProductAttributeDetailsByIdQuery, ProductAttributeDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetProductAttributeDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductAttributeDetailsDto> Handle(GetProductAttributeDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productAttribute = await _context.ProductAttributes.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Define model
            var model = new ProductAttributeDetailsDto
            {
                AttributeType = productAttribute.AttributeType,
                Name = productAttribute.Name,
                ProductAttributeId = productAttribute.ProductId,
                ProductId = productAttribute.ProductId,
            };

            //Retrun It
            return model;
        }
    }
}
