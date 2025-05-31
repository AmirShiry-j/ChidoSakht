using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductAttribute.Queries
{
    public class GetProductAttributeByIdQuery : IRequest<Domain.Products.ProductAttribute>
    {
        public int Id { get; set; }
        public GetProductAttributeByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetProductAttributeValueByIdQuery : IRequest<Domain.Products.ProductAttributeValue>
    {
        public int Id { get; set; }
        public GetProductAttributeValueByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetProductAttributesByProductIdQuery : IRequest<List<ProductAttributeDto>>
    {
        public int ProductId { get; set; }
        public GetProductAttributesByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
