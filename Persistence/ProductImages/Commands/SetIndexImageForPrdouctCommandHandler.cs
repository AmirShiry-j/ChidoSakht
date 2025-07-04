using Application.Store.AdminSection.ProductImageService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductImages.Commands
{
    public class SetIndexImageForPrdouctCommandHandler : IRequestHandler<SetIndexImageForPrdouctCommand>
    {
        private readonly DataBaseContext _context;
        public SetIndexImageForPrdouctCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(SetIndexImageForPrdouctCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.ProductImage);
            _context.Update(request.Product);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
