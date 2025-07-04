using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductVariant.Commands
{
    public class CreateProductVariantCommand : IRequest<int>
    {
        public CreateProductVariantDto ProductVariantDto { get; set; }
        public CreateProductVariantCommand(CreateProductVariantDto productVariantDto)
        {
            ProductVariantDto = productVariantDto;
        }
    }

    public class DeleteProductVariantCommand : IRequest
    {
        public Domain.Products.ProductVariant ProductVariant { get; set; }
        public DeleteProductVariantCommand(Domain.Products.ProductVariant ProductVariant)
        {
            this.ProductVariant = ProductVariant;
        }
    }

    public class UpdateProductVariantCommand : IRequest
    {
        public Domain.Products.ProductVariant ProductVariant { get; set; }

        public UpdateProductVariantCommand(Domain.Products.ProductVariant ProductVariant)
        {
            this.ProductVariant = ProductVariant;
        }
    }
}
