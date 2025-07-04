using Application.Store.AdminSection.ProductVariant.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductVariant.Commands
{
    public class DeleteProductVariantCommandHandler : IRequestHandler<DeleteProductVariantCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteProductVariantCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
        {
            _context.ProductVariants.Remove(request.ProductVariant);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
