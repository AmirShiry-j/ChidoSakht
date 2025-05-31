using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using Application.ProductAttribute.Commands;
using Application.ProductService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductAttribute.Queries
{
    public interface IProductAttributeQueriesService
    {
        Task<ResultDto<ProductAttributeDetailsDto>> GetOnProductAttribute(int ProductAttributeId);
    }
    public class ProductAttributeQueriesService : IProductAttributeQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductAttributeQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<ProductAttributeDetailsDto>> GetOnProductAttribute(int ProductAttributeId)
        {
            //get from db
            var productAttributeDetailsDto = await _mediator.Send(new GetProductAttributeDetailsByIdQuery((int)ProductAttributeId));
            if (productAttributeDetailsDto is null)
                return new ResultDto<ProductAttributeDetailsDto>
                {
                    MessageEventType = Common.MessageEventTypes.MessageEventType.NotFound
                };

            return new ResultDto<ProductAttributeDetailsDto>
            {
                IsSuccess = true,
                Data = productAttributeDetailsDto
            };
        }
    }
    public class ProductAttributeDetailsDto
    {
        public int ProductAttributeId { get; set; }
        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
        public int ProductId { get; set; }
        public List<Link> Links { get; set; }
    }
}
