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

            InfoForSimpleProductDto infoPrice = null;

            await _context.Database.ExecuteSqlInterpolatedAsync(
    $"UPDATE Products SET ViewCount = ViewCount + 1 WHERE Id = {request.Id}");

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

                if (product.ProductVariants.Any(p => p.ProductType.Equals(ProductType.Sample)))
                {
                    var sampleInfo = product.ProductVariants.SingleOrDefault();
                    infoPrice = new InfoForSimpleProductDto();

                    infoPrice.Price = sampleInfo.Price;
                    infoPrice.SpecialPrice = sampleInfo.SpecialPrice;
                    infoPrice.Stock = sampleInfo.Stock;
                }
            }
            else
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
                .Include(p => p.ProductVariants)
                 .ThenInclude(p => p.ProductVariantAttributeValues)
                .FirstOrDefaultAsync();
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
                ViewCount = product.ViewCount + 1,
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
                }).ToList(),
                productVariants = !product.ProductVariants.Where(p => p.ProductType.Equals(ProductType.Variable)).Any() ? null :
                product.ProductVariants.Where(p => p.ProductType.Equals(ProductType.Variable)).Select(s => new ProductVariantDto
                {
                    ProductVariantId = s.Id,
                    Price = s.Price,
                    SpecialPrice = s.SpecialPrice,
                    Stock = s.Stock,
                    ProductAttributeValues = s.ProductVariantAttributeValues.Select(v => new ProductAttributeValueDto
                    {
                        ProductAttributeValueId = v.ProductAttributeValueId,
                        Value = v.ProductAttributeValue.Value
                    }).ToList()
                }).ToList(),
                InfoForSimpleProduct = infoPrice
            };

            //Retrun It
            return model;
        }
    }
}
