using Application.ProductAttribute.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductAttribute.Commands
{
    public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteProductAttributeCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
        {
            _context.ProductAttributes.Remove(request.ProductAttribute);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
