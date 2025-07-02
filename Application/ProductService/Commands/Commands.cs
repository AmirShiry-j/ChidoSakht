using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductService.Commands
{
    public class InsertProductCommand : IRequest<int?>
    {
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public InsertProductCommand(string name, ProductType productType)
        {
            Name = name;
            ProductType = productType;
        }
    }
    public class UpdateProductCommand : IRequest
    {
        public Product Product { get; set; }

        public UpdateProductCommand(Product product)
        {
            Product = product;
        }
    }

    public class DeleteProductCommand : IRequest
    {
        public Product Product { get; set; }
        public DeleteProductCommand(Product Product)
        {
            this.Product = Product;
        }
    }

    
}
