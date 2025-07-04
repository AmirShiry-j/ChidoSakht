using Application.Store.AdminSection.ProductVariant.Queries;
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
    public class GetProductVariantsByProductIdQueryHandler : IRequestHandler<GetProductVariantsByProductIdQuery, List<ProductVariantDto>>
    {
        private readonly DataBaseContext _context;
        public GetProductVariantsByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductVariantDto>> Handle(GetProductVariantsByProductIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productVariants = await _context.ProductVariants
                .Where(p => p.ProductId.Equals(request.ProductId))
                .Include(p => p.ProductVariantAttributeValues)
                .ThenInclude(p => p.ProductAttributeValue)
                .ThenInclude(p => p.ProductAttribute)
                .Include(p => p.ProductVariantTransportation)
                .Select(p => new ProductVariantDto
                {
                    ProductVariantId = p.Id,
                    Price = p.Price,
                    SpecialPrice = p.SpecialPrice,
                    Stock = p.Stock,
                    Height = p.ProductVariantTransportation.Height,
                    Length = p.ProductVariantTransportation.Length,
                    Weight = p.ProductVariantTransportation.Weight,
                    Width = p.ProductVariantTransportation.Width,
                    ProductVariantAttributeValues = p.ProductVariantAttributeValues.Select(s => new ProductVariantAttributeValueDto
                    {
                        ProductAttributeId = s.ProductAttributeValue.ProductAttribute.Id,
                        ProductAttributeName = s.ProductAttributeValue.ProductAttribute.Name,
                        ProductAttributeValueId = s.ProductAttributeValue.Id,
                        ProductAttributeValue = s.ProductAttributeValue.Value,

                    }).ToList()
                })
                .ToListAsync();

            //Retrun It
            return productVariants;
        }
    }
}
