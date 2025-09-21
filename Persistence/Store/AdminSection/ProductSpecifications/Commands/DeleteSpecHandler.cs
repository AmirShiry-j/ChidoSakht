using Application.Store.AdminSection.ProductSpecificationService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductSpecifications.Commands
{

    public class DeleteSpecHandler : IRequestHandler<DeleteSpecCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteSpecHandler(DataBaseContext context) => _context = context;

        public async Task Handle(DeleteSpecCommand request, CancellationToken cancellationToken)
        {
            _context.ProductSpecifications.Remove(request.ProductSpecification);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
