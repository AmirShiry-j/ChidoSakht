using Application.Store.AdminSection.ProductSpecification.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecification.Commands
{
    public class UpdateSpecHandler : IRequestHandler<UpdateSpecCommand, bool>
    {
        private readonly DataBaseContext _context;
        public UpdateSpecHandler(DataBaseContext context) => _context = context;

        public async Task<bool> Handle(UpdateSpecCommand request, CancellationToken cancellationToken)
        {
            var spec = await _context.ProductSpecifications.FindAsync(new object[] { request.SpecId }, cancellationToken);
            if (spec == null) return false;
            spec.Key = request.Key;
            spec.Value = request.Value;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
