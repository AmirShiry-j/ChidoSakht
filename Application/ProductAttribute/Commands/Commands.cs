using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductAttribute.Commands
{
    public class CreateProductAttributeCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
        public CreateProductAttributeCommand(int productId, string name, AttributeType attributeType)
        {
            ProductId = productId;
            Name = name;
            AttributeType = attributeType;
        }
    }

    public class UpdateProductAttributeCommand : IRequest
    {
        public Domain.Products.ProductAttribute ProductAttribute { get; set; }

        public UpdateProductAttributeCommand(Domain.Products.ProductAttribute productAttribute)
        {
            ProductAttribute = productAttribute;
        }
    }

    public class DeleteProductAttributeCommand : IRequest
    {
        public Domain.Products.ProductAttribute ProductAttribute { get; set; }

        public DeleteProductAttributeCommand(Domain.Products.ProductAttribute productAttribute)
        {
            ProductAttribute = productAttribute;
        }
    }


    public class CreateValueForProductAttributeCommand : IRequest<int>
    {
        public int ProductAttributeId { get; set; }
        public string Value { get; set; }
        public CreateValueForProductAttributeCommand(int productAttributeId, string value)
        {
            ProductAttributeId = productAttributeId;
            Value = value;
        }
    }
}
