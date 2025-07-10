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

namespace Persistence.Store.UserSection.Products.Queries
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

            InfoPriceDto infoPrice = null;

            if (product.ProductType == ProductType.Sample)
            {
                product = await _context.Products
                .Where(p => p.Id.Equals(request.Id))
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .ThenInclude(p => p.ProductAttributeValues)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSpecificationGroups)
                .ThenInclude(p => p.Specifications)
                .Include(p => p.ProductVariants)
                .ThenInclude(p => p.ProductVariantTransportation)
                .FirstOrDefaultAsync();

                if (product.ProductVariants.Any())
                {
                    var sampleInfo = product.ProductVariants.SingleOrDefault();
                    infoPrice = new InfoPriceDto();

                    infoPrice.Price = sampleInfo.Price;
                    infoPrice.SpecialPrice = sampleInfo.SpecialPrice;
                    infoPrice.Stock = sampleInfo.Stock;
                }
            }
            else
            {
                product = await _context.Products
                    .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .ThenInclude(p => p.ProductAttributeValues)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSpecificationGroups)
                .ThenInclude(p => p.Specifications)
                .Include(p => p.ProductVariants)
                .ThenInclude(p => p.ProductVariantTransportation)
                .FirstOrDefaultAsync();

                if (product.ProductVariants.Any())
                {
                    var sampleInfo = product.ProductVariants.FirstOrDefault();
                    infoPrice = new InfoPriceDto();

                    infoPrice.Price = sampleInfo.Price;
                    infoPrice.SpecialPrice = sampleInfo.SpecialPrice;
                    infoPrice.Stock = sampleInfo.Stock;

                    infoPrice.HasDiscount = sampleInfo.SpecialPrice is not null;
                    if (infoPrice.HasDiscount)
                    {
                        //infoPrice.PercentDiscount = int.Parse(((infoPrice.Price - infoPrice.SpecialPrice) / infoPrice.Price) * 100));
                        infoPrice.PercentDiscount = 10;
                    }
                }
            }

            var model = new ProductWithDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = product.ProductType,
                ProductTypeName = product.ProductType == ProductType.Sample ? "Sample" : "Variable",
                Description = product.Description,
                UniqeLink = product.UniqeLink,
                ImageAltText = product.ImageAltText,
                NameIndexImage = product.NameIndexImage,
                UniCode = product.UniCode,
                CategoryId = product?.CategoryId,
                CategoryName = product.Category?.Name,
                Price = infoPrice?.Price,
                SpecialPrice = infoPrice?.SpecialPrice,
                Stock = infoPrice?.Stock,
                AttributeAndValues = product.ProductAttributes.Select(p => new ProductAttributeAndValuesDto
                {
                    ProductAttributeId = p.Id,
                    Name = p.Name,
                    UseForVariant = p.UseForVariant,
                    AttributeType = p.AttributeType,
                    Values = p.ProductAttributeValues.Select(s => new ProductAttributeValueDto
                    {
                        ProductAttributeValueId = s.Id,
                        Value = s.Value
                    }).ToList()
                }).ToList(),
                SpecificationGroups = product.ProductSpecificationGroups.Select(s => new SpecGroupWithSpecsDto
                {
                    GroupId = s.Id,
                    Title = s.Title,
                    Specifications = s.Specifications.Select(w => new SpecDto
                    {
                        SpecId = w.Id,
                        Key = w.Key,
                        Value = w.Value
                    }).ToList()

                }).ToList(),
                ProductImages = product.ProductImages.Select(s => new ProductImageDto
                {
                    Id = s.Id,
                    IsIndex = s.IsIndex,
                    Name = s.Name
                }).ToList()
            };

            //Retrun It
            return model;
        }
    }
    public class InfoPriceDto
    {
        public long? Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int? Stock { get; set; }
        public bool HasDiscount { get; set; }
        public int PercentDiscount { get; set; }
    }
}
