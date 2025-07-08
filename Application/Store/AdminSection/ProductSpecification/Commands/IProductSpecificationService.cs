using Application.Store.AdminSection.ProductSpecification.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecification.Commands
{
    public interface IProductSpecificationService
    {
        // Groups
        Task<long> CreateSpecGroupAsync(int productId, string title);
        Task<bool> UpdateSpecGroupAsync(long groupId, string title);
        Task<bool> DeleteSpecGroupAsync(long groupId);
        Task<List<SpecGroupDto>> GetSpecGroupsAsync(int productId);

        // Specs
        Task<long> CreateSpecAsync(long groupId, string key, string value);
        Task<bool> UpdateSpecAsync(long specId, string key, string value);
        Task<bool> DeleteSpecAsync(long specId);
    }

    public class ProductSpecificationService : IProductSpecificationService
    {
        private readonly IMediator _mediator;
        public ProductSpecificationService(IMediator mediator) => _mediator = mediator;

        public Task<long> CreateSpecGroupAsync(int productId, string title)
            => _mediator.Send(new CreateSpecGroupCommand(productId, title));

        public Task<bool> UpdateSpecGroupAsync(long groupId, string title)
            => _mediator.Send(new UpdateSpecGroupCommand(groupId, title));

        public Task<bool> DeleteSpecGroupAsync(long groupId)
            => _mediator.Send(new DeleteSpecGroupCommand(groupId));

        public Task<List<SpecGroupDto>> GetSpecGroupsAsync(int productId)
            => _mediator.Send(new GetSpecGroupsQuery(productId));

        public Task<long> CreateSpecAsync(long groupId, string key, string value)
            => _mediator.Send(new CreateSpecCommand(groupId, key, value));

        public Task<bool> UpdateSpecAsync(long specId, string key, string value)
            => _mediator.Send(new UpdateSpecCommand(specId, key, value));

        public Task<bool> DeleteSpecAsync(long specId)
            => _mediator.Send(new DeleteSpecCommand(specId));
    }
}
