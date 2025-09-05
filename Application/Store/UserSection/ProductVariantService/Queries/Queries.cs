using Application.Store.AdminSection.ProductVariant.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.ProductVariantService.Queries
{
    public class GetProductVariantByInUserSectionIdQuery : IRequest<ProductVariant>
    {
        public int ProductVariantId { get; set; }
        public GetProductVariantByInUserSectionIdQuery(int ProductVariantId)
        {
            this.ProductVariantId = ProductVariantId;
        }
    }

    public class GetProductVariantTypeSampleByProductIdInUserSectionQuery : IRequest<ProductVariant>
    {
        public int ProductId { get; set; }
        public GetProductVariantTypeSampleByProductIdInUserSectionQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
