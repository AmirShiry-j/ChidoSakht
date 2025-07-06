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
}
