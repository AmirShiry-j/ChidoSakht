using Application.ProductAttribute.Commands;
using Domain.Products;
using MediatR;
using Persistence.Contexts;

namespace Persistence.ProductAttribute.Commands
{
    public class CreateProductAttributeCommandHandler : IRequestHandler<CreateProductAttributeCommand, int>
    {
        private readonly DataBaseContext _context;
        public CreateProductAttributeCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProductAttributeCommand request, CancellationToken cancellationToken)
        {
            //Define
            var newProductAttribute = new Domain.Products.ProductAttribute();
            newProductAttribute.ProductId = request.ProductId;
            newProductAttribute.AttributeType = request.AttributeType;
            newProductAttribute.Name = request.Name;
            newProductAttribute.UseForVariant = request.UseForVariant;

            //Add and save in DB
            await _context.ProductAttributes.AddAsync(newProductAttribute);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return newProductAttribute.Id;
        }
    }
}
