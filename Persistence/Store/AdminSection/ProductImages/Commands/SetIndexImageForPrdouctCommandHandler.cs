using Application.Store.AdminSection.ProductImageService.Commands;
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
    public class SetIndexImageForPrdouctCommandHandler : IRequestHandler<SetIndexImageForPrdouctCommand>
    {
        private readonly DataBaseContext _context;
        public SetIndexImageForPrdouctCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(SetIndexImageForPrdouctCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductImage is not null)
                _context.Update(request.ProductImage);

            var images = _context.ProductImages.IgnoreQueryFilters().Where(p => p.ProductId.Equals(request.Product.Id) && p.Name != request.Product.NameIndexImage);
            foreach (var item in images)
            {
                item.IsIndex = false;
                _context.Update(item);
            }

            _context.Update(request.Product);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
