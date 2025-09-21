using Application.Store.AdminSection.ProductVariant.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductVariant.Queries
{
    public class GetProductVariantsTypeVariableByProductIdQueryHandler : IRequestHandler<GetProductVariantsTypeVariableByProductIdQuery, List<ProductVariantDto>>
    {
        private readonly DataBaseContext _context;
        public GetProductVariantsTypeVariableByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductVariantDto>> Handle(GetProductVariantsTypeVariableByProductIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productVariants = await _context.ProductVariants.IgnoreQueryFilters()
                .Where(p => p.ProductId.Equals(request.ProductId) && p.ProductType.Equals(ProductType.Variable))
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
