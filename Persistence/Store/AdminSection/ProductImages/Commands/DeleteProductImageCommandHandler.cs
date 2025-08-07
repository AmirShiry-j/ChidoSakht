using Application.Store.AdminSection.ProductImageService.Commands;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductImages.Commands
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
            //Update product if image was set for Index
            if (request.ProductImage.IsIndex)
            {
                var product = await _context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id.Equals(request.ProductImage.ProductId));
                product.NameIndexImage = null;
                _context.Products.Update(product);
            }

            _context.ProductImages.Remove(request.ProductImage);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
