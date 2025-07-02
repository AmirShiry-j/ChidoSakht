using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductImageService.Queries
{
    public class GetProductImagesQueries : IRequest<List<ProductImageDto>>
    {
        public int ProductId { get; set; }

        public GetProductImagesQueries(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetAProductImageByNameQuery : IRequest<ProductImage>
    {
        public string Name { get; set; }

        public GetAProductImageByNameQuery(string name)
        {
            Name = name;
        }
    }
}
