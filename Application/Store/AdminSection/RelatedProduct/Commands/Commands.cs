using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.RelatedProduct.Commands
{
    public class AddRelatedProductCommand : IRequest
    {
        public int ProductId { get; set; }
        public int RelatedProductId { get; set; }

        public AddRelatedProductCommand(int productId, int relatedProductId)
        {
            ProductId = productId;
            RelatedProductId = relatedProductId;
        }
    }

    public class RemoveRelatedProductCommand : IRequest
    {
        public int ProductId { get; set; }
        public int RelatedProductId { get; set; }

        public RemoveRelatedProductCommand(int productId, int relatedProductId)
        {
            ProductId = productId;
            RelatedProductId = relatedProductId;
        }
    }

}
