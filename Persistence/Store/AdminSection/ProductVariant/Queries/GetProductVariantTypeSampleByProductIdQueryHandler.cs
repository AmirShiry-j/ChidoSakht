using Application.Store.AdminSection.ProductVariant.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductVariant.Queries
{
    public class GetProductVariantTypeSampleByProductIdQueryHandler : IRequestHandler<GetProductVariantTypeSampleByProductIdQuery, ProductVariantDto>
    {
        private readonly DataBaseContext _context;
        public GetProductVariantTypeSampleByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductVariantDto> Handle(GetProductVariantTypeSampleByProductIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productVariant = await _context.ProductVariants.IgnoreQueryFilters()
                .Where(p => p.ProductId.Equals(request.ProductId) && p.ProductType.Equals(ProductType.Sample))
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
                    Width = p.ProductVariantTransportation.Width
                })
                .SingleOrDefaultAsync();

            //Retrun It
            return productVariant;
        }
    }
}
