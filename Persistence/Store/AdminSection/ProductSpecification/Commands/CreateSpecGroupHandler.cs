using Application.Store.AdminSection.ProductSpecification.Commands;
using Domain.Products;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecification.Commands
{
    public class CreateSpecGroupHandler : IRequestHandler<CreateSpecGroupCommand, long>
    {
        private readonly DataBaseContext _context;
        public CreateSpecGroupHandler(DataBaseContext context) => _context = context;

        public async Task<long> Handle(CreateSpecGroupCommand request, CancellationToken cancellationToken)
        {
            var group = new ProductSpecificationGroup
            {
                ProductId = request.ProductId,
                Title = request.Title
            };
            _context.ProductSpecificationGroups.Add(group);
            await _context.SaveChangesAsync(cancellationToken);
            return group.Id;
        }
    }
}
