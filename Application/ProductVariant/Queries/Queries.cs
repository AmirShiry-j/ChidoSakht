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
}
