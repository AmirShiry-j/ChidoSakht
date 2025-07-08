using Application.Store.AdminSection.ProductSpecification.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecification.Commands
{
    public class UpdateSpecGroupHandler : IRequestHandler<UpdateSpecGroupCommand, bool>
    {
        private readonly DataBaseContext _context;
        public UpdateSpecGroupHandler(DataBaseContext context) => _context = context;

        public async Task<bool> Handle(UpdateSpecGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.ProductSpecificationGroups.FindAsync(new object[] { request.GroupId }, cancellationToken);
            if (group == null) return false;
            group.Title = request.Title;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
