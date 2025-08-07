using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.ProductSpecificationService.Queries
{
    public interface IProductSpecificationQueriesService
    {
        Task<ResultDto<List<SpecGroupWithSpecsDto>>> GetSpecGroupsWihtSpecsAsync(int ProductId);
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

        public async Task<ResultDto<List<SpecGroupWithSpecsDto>>> GetSpecGroupsWihtSpecsAsync(int ProductId)
        {

            //check exist
            var product = await _mediator.Send(new GetProductByIdInUserSectionQuery(ProductId));
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
    }
    public class SpecGroupWithSpecsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<SpecDto> Specifications { get; set; }
    }

    public class SpecDto
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}