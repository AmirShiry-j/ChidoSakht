using Application.ProductService.Commands;
using Domain.Products;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteProductCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _context.Products.Remove(request.Product);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
