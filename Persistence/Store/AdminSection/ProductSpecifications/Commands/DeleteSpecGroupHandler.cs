using Application.Store.AdminSection.ProductSpecificationService.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Commands
{
    public class DeleteSpecGroupHandler : IRequestHandler<DeleteSpecGroupCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteSpecGroupHandler(DataBaseContext context) => _context = context;

        public async Task Handle(DeleteSpecGroupCommand request, CancellationToken cancellationToken)
        {
            _context.ProductSpecificationGroups.Remove(request.ProductSpecificationGroup);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
