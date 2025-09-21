using Application.Store.AdminSection.ProductSpecificationService.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecifications.Commands
{
    public class UpdateSpecGroupHandler : IRequestHandler<UpdateSpecGroupCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateSpecGroupHandler(DataBaseContext context) => _context = context;

        public async Task Handle(UpdateSpecGroupCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.ProductSpecificationGroup);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
