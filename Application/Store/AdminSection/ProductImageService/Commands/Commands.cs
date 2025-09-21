using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductImageService.Commands
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
        public Product Product { get; set; }
        public SetIndexImageForPrdouctCommand(ProductImage productImage, Product product)
        {
            ProductImage = productImage;
            Product = product;
        }
    }

    public class DeleteProductImagesByProductIdCommand : IRequest
    {
        public int ProductId { get; set; }
        public string BasePathImages { get; set; }
        public DeleteProductImagesByProductIdCommand(int ProductId, string basePathImages)
        {
            this.ProductId = ProductId;
            BasePathImages = basePathImages;
        }
    }
}
