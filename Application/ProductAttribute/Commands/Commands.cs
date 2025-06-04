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
        public bool UseForVariant { get; set; }
        public CreateProductAttributeCommand(int productId, string name, AttributeType attributeType, bool useForVariant)
        {
            ProductId = productId;
            Name = name;
            AttributeType = attributeType;
            UseForVariant = useForVariant;
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


    public class CreateProductAttributeValueCommand : IRequest<int>
    {
        public int ProductAttributeId { get; set; }
        public string Value { get; set; }
        public CreateProductAttributeValueCommand(int productAttributeId, string value)
        {
            ProductAttributeId = productAttributeId;
            Value = value;
        }
    }

    public class DeleteProductAttributeValueCommand : IRequest
    {
        public Domain.Products.ProductAttributeValue ProductAttributeValue { get; set; }

        public DeleteProductAttributeValueCommand(Domain.Products.ProductAttributeValue productAttributeValue)
        {
            ProductAttributeValue = productAttributeValue;
        }
    }

    public class UpdateProductAttributeValueCommand : IRequest
    {
        public Domain.Products.ProductAttributeValue ProductAttributeValue { get; set; }

        public UpdateProductAttributeValueCommand(Domain.Products.ProductAttributeValue productAttributeValue)
        {
            ProductAttributeValue = productAttributeValue;
        }
    }
}
