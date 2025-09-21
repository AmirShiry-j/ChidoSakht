using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.RelatedProduct.Queries
{
    public class GetExistingRelationsQuery : IRequest<List<Domain.Products.RelatedProduct>>
    {
        public int ProductId { get; set; }
        public List<int> RelatedIds { get; set; }

        public GetExistingRelationsQuery(int productId, List<int> relatedIds)
        {
            ProductId = productId;
            RelatedIds = relatedIds;
        }
    }

    public class GetRelatedProductsQuery : IRequest<List<ProductDto>>
    {
        public int ProductId { get; }

        public GetRelatedProductsQuery(int productId)
        {
            ProductId = productId;
        }
    }
    public class GetExistingProductIdsForSetRelatedQuery : IRequest<List<int>>
    {
        public List<int> RecivedIds { get; }
        public GetExistingProductIdsForSetRelatedQuery(List<int> recivedIds)
        {
            RecivedIds = recivedIds;
        }
    }
}
