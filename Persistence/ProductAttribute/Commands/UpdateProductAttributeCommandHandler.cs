using Application.ProductAttribute.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductAttribute.Commands
{
    public class UpdateProductAttributeCommandHandler : IRequestHandler<UpdateProductAttributeCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateProductAttributeCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateProductAttributeCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.ProductAttribute);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
