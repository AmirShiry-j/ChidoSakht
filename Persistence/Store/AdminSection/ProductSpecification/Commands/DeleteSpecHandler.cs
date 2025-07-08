using Application.Store.AdminSection.ProductSpecification.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductSpecification.Commands
{

    public class DeleteSpecHandler : IRequestHandler<DeleteSpecCommand, bool>
    {
        private readonly DataBaseContext _context;
        public DeleteSpecHandler(DataBaseContext context) => _context = context;

        public async Task<bool> Handle(DeleteSpecCommand request, CancellationToken cancellationToken)
        {
            var spec = await _context.ProductSpecifications.FindAsync(new object[] { request.SpecId }, cancellationToken);
            if (spec == null) return false;

            _context.ProductSpecifications.Remove(spec);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
