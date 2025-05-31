using Application.ProductAttribute.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductAttribute.Commands
{
    public class CreateValueForProductAttributeCommandHandler : IRequestHandler<CreateValueForProductAttributeCommand, int>
    {
        private readonly DataBaseContext _context;
        public CreateValueForProductAttributeCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateValueForProductAttributeCommand request, CancellationToken cancellationToken)
        {
            //Define
            var newValue = new Domain.Products.ProductAttributeValue();
            newValue.ProductAttributeId = request.ProductAttributeId;
            newValue.Value = request.Value;

            //Add and save in DB
            await _context.ProductAttributeValues.AddAsync(newValue);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return newValue.Id;
        }
    }
}
