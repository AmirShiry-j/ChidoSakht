using Application.Store.AdminSection.ProductService.Queries;
using Domain.Categories;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.Products.Queries
{
    public class GetProductDetailsByIdQueryHandler : IRequestHandler<GetProductDetailsByIdQuery, ProductDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetProductDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<ProductDetailsDto> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            Product product = await _context.Products.IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            if (product is null)
                return null;

            InfoForSampleProductDto infoForSampleProduct = null;
            if (product.ProductType == ProductType.Sample)
            {
                product = await _context.Products.IgnoreQueryFilters()
                .Where(p => p.Id.Equals(request.Id))
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                .ThenInclude(p => p.ProductVariantTransportation)
                .FirstOrDefaultAsync();

                if (product.ProductVariants.Any())
                {
                    var sampleInfo = product.ProductVariants.SingleOrDefault();
                    infoForSampleProduct = new InfoForSampleProductDto();

                    infoForSampleProduct.Price = sampleInfo.Price;
                    infoForSampleProduct.SpecialPrice = sampleInfo.SpecialPrice;
                    infoForSampleProduct.Stock = sampleInfo.Stock;
                    if (sampleInfo.ProductVariantTransportation is not null)
                    {
                        infoForSampleProduct.Weight = sampleInfo.ProductVariantTransportation.Weight;
                        infoForSampleProduct.Width = sampleInfo.ProductVariantTransportation.Width;
                        infoForSampleProduct.Length = sampleInfo.ProductVariantTransportation.Length;
                        infoForSampleProduct.Height = sampleInfo.ProductVariantTransportation.Height;
                    }
                }
            }
            else
            {
                product = await _context.Products.IgnoreQueryFilters()
                    .Where(p => p.Id.Equals(request.Id))
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync();
            }

            var model = new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = product.ProductType,
                CreateTime = product.CreateTime,
                Description = product.Description,
                LastUpdateTime = product.LastUpdateTime,
                UniqeLink = product.UniqeLink,
                ImageAltText = product.ImageAltText,
                NameIndexImage = product.NameIndexImage,
                UniCode = product.UniCode,
                CategoryId = product?.CategoryId,
                CategoryName = product.Category?.Name,
                InfoForSampleProduct = infoForSampleProduct,
                ViewCount = product.ViewCount,
                IsPublished = product.IsPublished
            };

            //Retrun It
            return model;
        }
    }
}