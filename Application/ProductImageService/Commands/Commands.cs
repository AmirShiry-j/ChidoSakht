using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductImageService.Commands
{
    public class CreateProductImageCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public CreateProductImageCommand(int productId, string name)
        {
            ProductId = productId;
            Name = name;
        }
    }

    public class DeleteProductImageCommand : IRequest
    {
        public ProductImage ProductImage { get; set; }
        public DeleteProductImageCommand(ProductImage productImage)
        {
            ProductImage = productImage;
        }
    }

    public class SetIndexImageForPrdouctCommand : IRequest
    {
        public ProductImage ProductImage { get; set; }
        public SetIndexImageForPrdouctCommand(ProductImage productImage)
        {
            ProductImage = productImage;
        }
    }
}
