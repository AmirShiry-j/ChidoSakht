using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Store.AdminSection.ProductSpecificationService.Queries.ProductSpecificationQueriesService;

namespace Application.Store.AdminSection.ProductSpecificationService.Queries
{
    public interface IProductSpecificationQueriesService
    {
        Task<ResultDto<List<SpecGroupWithSpecsDto>>> GetSpecGroupsWihtSpecsAsync(int ProductId);
        Task<ResultDto<List<SpecGroupDto>>> GetSpecGroupsByProductIdAsync(int ProductId);
        Task<ResultDto<List<SpecDto>>> GetSpecsBySpecGroupIdAsync(long SpecGroupId);
    }
    public class ProductSpecificationQueriesService : IProductSpecificationQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductSpecificationQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<List<SpecGroupDto>>> GetSpecGroupsByProductIdAsync(int ProductId)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<List<SpecGroupDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var goups = await _mediator.Send(new GetSpecGroupsWithProductIdQuery(ProductId));

            return new ResultDto<List<SpecGroupDto>>
            {
                IsSuccess = true,
                Data = goups
            };
        }

        public async Task<ResultDto<List<SpecGroupWithSpecsDto>>> GetSpecGroupsWihtSpecsAsync(int ProductId)
        {

            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<List<SpecGroupWithSpecsDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var goupsAndSpecs = await _mediator.Send(new GetAllSpecGroupsWithSpecsByProductIdQuery(ProductId));

            return new ResultDto<List<SpecGroupWithSpecsDto>>
            {
                IsSuccess = true,
                Data = goupsAndSpecs
            };
        }

        public async Task<ResultDto<List<SpecDto>>> GetSpecsBySpecGroupIdAsync(long SpecGroupId)
        {
            //check exist
            var group = await _mediator.Send(new GetSpecGroupByIdQuery(SpecGroupId));
            if (group is null)
            {
                return new ResultDto<List<SpecDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var specs = await _mediator.Send(new GetSpecsByGroupSpecIdQuery(SpecGroupId));

            return new ResultDto<List<SpecDto>>
            {
                IsSuccess = true,
                Data = specs
            };
        }
    }
    public class SpecGroupWithSpecsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<SpecDto> Specifications { get; set; }
    }
    public class SpecGroupDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
    public class SpecDto
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}