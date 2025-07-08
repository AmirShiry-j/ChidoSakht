using Application.Store.AdminSection.ProductSpecificationService.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Commands
{
    public class UpdateSpecHandler : IRequestHandler<UpdateSpecCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateSpecHandler(DataBaseContext context) => _context = context;

        public async Task Handle(UpdateSpecCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.ProductSpecification);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
