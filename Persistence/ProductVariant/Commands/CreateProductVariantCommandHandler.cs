using Application.Store.AdminSection.ProductVariant.Commands;
using Domain.Products;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductVariant.Commands
{
    public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, int>
    {
        private readonly DataBaseContext _context;
        public CreateProductVariantCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
        {
            //Define
            var productVariantAttributeValues = new List<ProductVariantAttributeValue>();
            if (request.ProductVariantDto.ProductAttributeValueIds is not null && request.ProductVariantDto.ProductAttributeValueIds.Any())
            {
                productVariantAttributeValues = request.ProductVariantDto.ProductAttributeValueIds.Select(p => new ProductVariantAttributeValue
                {
                    ProductAttributeValueId = p
                }).ToList();
            }

            var newProductVariant = new Domain.Products.ProductVariant()
            {
                ProductId = request.ProductVariantDto.ProductId,
                Price = request.ProductVariantDto.Price,
                SpecialPrice = request.ProductVariantDto.SpecialPrice,
                ProductVariantAttributeValues = productVariantAttributeValues,
                Stock = request.ProductVariantDto.Stock,
                ProductType = request.ProductVariantDto.ProductType,
                ProductVariantTransportation = new ProductVariantTransportation
                {
                    Width = request.ProductVariantDto.Width,
                    Weight = request.ProductVariantDto.Weight,
                    Length = request.ProductVariantDto.Length,
                    Height = request.ProductVariantDto.Height,
                }
            };

            //Add and save in DB
            await _context.ProductVariants.AddAsync(newProductVariant);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return newProductVariant.Id;
        }
    }
}
