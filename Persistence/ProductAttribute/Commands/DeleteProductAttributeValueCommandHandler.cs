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
    public class DeleteProductAttributeValueCommandHandler : IRequestHandler<DeleteProductAttributeValueCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteProductAttributeValueCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductAttributeValueCommand request, CancellationToken cancellationToken)
        {
            _context.ProductAttributeValues.Remove(request.ProductAttributeValue);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
