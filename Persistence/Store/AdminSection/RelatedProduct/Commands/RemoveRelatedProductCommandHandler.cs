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
    public class RemoveRelatedProductCommandHandler : IRequestHandler<RemoveRelatedProductCommand>
    {
        private readonly DataBaseContext _context;
        public RemoveRelatedProductCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveRelatedProductCommand request, CancellationToken cancellationToken)
        {
            var relation = await _context.Set<Domain.Products.RelatedProduct>().IgnoreQueryFilters()
                .FirstOrDefaultAsync(r =>
                    r.ProductId == request.ProductId &&
                    r.RelatedProductId == request.RelatedProductId,
                    cancellationToken);

            if (relation != null)
            {
                _context.Remove(relation);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }
    }

}
