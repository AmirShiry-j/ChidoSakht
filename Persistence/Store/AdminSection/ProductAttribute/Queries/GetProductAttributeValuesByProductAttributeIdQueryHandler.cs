using Application.Store.AdminSection.ProductAttribute.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductAttribute.Queries
{
    public class GetProductAttributeValuesByProductAttributeIdQueryHandler : IRequestHandler<GetProductAttributeValuesByProductAttributeIdQuery, List<ProductAttributeValueDto>>
    {
        private readonly DataBaseContext _context;
        public GetProductAttributeValuesByProductAttributeIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductAttributeValueDto>> Handle(GetProductAttributeValuesByProductAttributeIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productAttributeValues = await _context.ProductAttributeValues.Where(p => p.ProductAttributeId.Equals(request.ProductAttributeId))
                .Select(p => new ProductAttributeValueDto
                {
                    ProductAttributeValueId = p.Id,
                    Value = p.Value
                }).ToListAsync();

            //Retrun It
            return productAttributeValues;
        }
    }
}
