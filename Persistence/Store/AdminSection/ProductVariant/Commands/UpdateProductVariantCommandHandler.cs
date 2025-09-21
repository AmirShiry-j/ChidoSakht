using Application.Store.AdminSection.ProductVariant.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductVariant.Commands
{
    public class UpdateProductVariantCommandHandler : IRequestHandler<UpdateProductVariantCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateProductVariantCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.ProductVariant);
            _context.SaveChanges();
        }
    }
}
