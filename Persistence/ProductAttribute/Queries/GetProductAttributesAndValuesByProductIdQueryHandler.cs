using Application.Store.AdminSection.ProductAttribute.Queries;
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
    public class GetProductAttributesAndValuesByProductIdQueryHandler : IRequestHandler<GetProductAttributesAndValuesByProductIdQuery, List<ProductAttributeAndValuesDto>>
    {
        private readonly DataBaseContext _context;
        public GetProductAttributesAndValuesByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductAttributeAndValuesDto>> Handle(GetProductAttributesAndValuesByProductIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productAttributesAndValues = await _context.ProductAttributes.Where(p => p.ProductId.Equals(request.ProductId))
                .Include(p => p.ProductAttributeValues)
                .Select(p => new ProductAttributeAndValuesDto
                {
                    AttributeType = p.AttributeType,
                    Name = p.Name,
                    ProductAttributeId = p.Id,
                    UseForVariant = p.UseForVariant,
                    Values = p.ProductAttributeValues.Select(c => new ProductAttributeValueDto
                    {
                        ProductAttributeValueId = c.Id,
                        Value = c.Value
                    }).ToList()

                }).ToListAsync();

            //Retrun It
            return productAttributesAndValues;
        }
    }
}
