using Application.Store.UserSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Queries
{
    public class GetOneProductWithDetailsByIdQueryHandler : IRequestHandler<GetOneProductWithDetailsByIdQuery, ProductWithDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetOneProductWithDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductWithDetailsDto> Handle(GetOneProductWithDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            Product product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            if (product is null)
                return null;

            InfoForSampleProductDto infoForSampleProduct = null;
            if (product.ProductType == ProductType.Sample)
            {
                product = await _context.Products
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
                product = await _context.Products
                    .Where(p => p.Id.Equals(request.Id))
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync();
            }

            var model = new ProductWithDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = product.ProductType,
                Description = product.Description,
                UniqeLink = product.UniqeLink,
                ImageAltText = product.ImageAltText,
                NameIndexImage = product.NameIndexImage,
                UniCode = product.UniCode,
                CategoryId = product?.CategoryId,
                CategoryName = product.Category?.Name,
                InfoForSampleProduct = infoForSampleProduct
            };

            //Retrun It
            return model;
        }
    }
}
