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
    public class UpdateProductAttributeValueCommandHandler : IRequestHandler<UpdateProductAttributeValueCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateProductAttributeValueCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateProductAttributeValueCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.ProductAttributeValue);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
