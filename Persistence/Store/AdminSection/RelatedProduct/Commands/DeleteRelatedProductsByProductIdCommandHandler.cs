using Application.Store.AdminSection.RelatedProduct.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.RelatedProduct.Commands
{
    public class DeleteRelatedProductsByProductIdCommandHandler : IRequestHandler<DeleteRelatedProductsByProductIdCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteRelatedProductsByProductIdCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteRelatedProductsByProductIdCommand request, CancellationToken cancellationToken)
        {
            var relateds = await _context.RelatedProducts.IgnoreQueryFilters()
                .Where(p => p.ProductId.Equals(request.ProductId) || p.RelatedProductId.Equals(request.ProductId))
                .ToListAsync();

            if (relateds.Any())
            {
                _context.RelatedProducts.RemoveRange(relateds);
                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}
