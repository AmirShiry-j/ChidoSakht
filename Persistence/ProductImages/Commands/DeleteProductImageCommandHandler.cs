using Application.ProductImageService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductImages.Commands
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteProductImageCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            _context.ProductImages.Remove(request.ProductImage);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
