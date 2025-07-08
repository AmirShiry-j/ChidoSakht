using Application.Store.AdminSection.ProductSpecification.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecification.Commands
{
    public class DeleteSpecGroupHandler : IRequestHandler<DeleteSpecGroupCommand, bool>
    {
        private readonly DataBaseContext _context;
        public DeleteSpecGroupHandler(DataBaseContext context) => _context = context;

        public async Task<bool> Handle(DeleteSpecGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.ProductSpecificationGroups
                .Include(g => g.Specifications)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (group == null) return false;

            _context.ProductSpecificationGroups.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
