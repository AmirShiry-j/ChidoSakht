using Application.Store.AdminSection.ProductSpecification.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.ProductSpecification.Commands
{
    ////
    ///


    public class CreateSpecHandler : IRequestHandler<CreateSpecCommand, long>
    {
        private readonly DataBaseContext _context;
        public CreateSpecHandler(DataBaseContext context) => _context = context;

        public async Task<long> Handle(CreateSpecCommand request, CancellationToken cancellationToken)
        {
            var spec = new Domain.Products.ProductSpecification
            {
                ProductSpecificationGroupId = request.GroupId,
                Key = request.Key,
                Value = request.Value
            };
            _context.ProductSpecifications.Add(spec);
            await _context.SaveChangesAsync(cancellationToken);
            return spec.Id;
        }
    }
}
