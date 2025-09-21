using Application.Store.AdminSection.RelatedProduct.Commands;
using Application.Store.AdminSection.RelatedProduct.Queries;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.RelatedProduct.Commands
{
    public class AddRelatedProductCommandHandler : IRequestHandler<AddRelatedProductCommand>
    {
        private readonly DataBaseContext _context;
        public AddRelatedProductCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task Handle(AddRelatedProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Products.RelatedProduct
            {
                ProductId = request.ProductId,
                RelatedProductId = request.RelatedProductId
            };

            _context.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }

    }

}
