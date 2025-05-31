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
}
