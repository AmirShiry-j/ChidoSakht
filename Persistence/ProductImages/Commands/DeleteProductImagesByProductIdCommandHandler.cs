using Application.ProductImageService.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductImages.Commands
{
    public class DeleteProductImagesByProductIdCommandHandler : IRequestHandler<DeleteProductImagesByProductIdCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteProductImagesByProductIdCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductImagesByProductIdCommand request, CancellationToken cancellationToken)
        {
            var images = await _context.ProductImages.Where(p => p.ProductId.Equals(request.ProductId)).ToListAsync();

            foreach (var image in images)
            {
                var fullPath = Path.Combine(request.BasePathImages, image.Name);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (images.Any())
            {
                _context.ProductImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}
