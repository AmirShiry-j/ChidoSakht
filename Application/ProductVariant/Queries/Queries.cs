using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant.Queries
{
    public class GetValidSendedProductAttributeValueIdsByProductIdQuery : IRequest<List<int>>
    {
        public int ProductId { get; set; }
        public List<int> ProductAttributeValueIds { get; set; }
        public GetValidSendedProductAttributeValueIdsByProductIdQuery(int productId, List<int> productAttributeValueIds)
        {
            ProductId = productId;
            ProductAttributeValueIds = productAttributeValueIds;
        }
    }
    public class GetProductVariantByIdQuery : IRequest<Domain.Products.ProductVariant>
    {
        public int ProductVariantId { get; set; }
        public GetProductVariantByIdQuery(int ProductVariantId)
        {
            this.ProductVariantId = ProductVariantId;
        }
    }
    public class GetProductVariantsByProductIdQuery : IRequest<List<ProductVariantDto>>
    {
        public int ProductId { get; set; }
        public GetProductVariantsByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
    }

}
