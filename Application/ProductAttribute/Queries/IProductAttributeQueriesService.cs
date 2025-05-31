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
        Task<ResultDto<List<ProductAttributeDto>>> GetProductAttributesByProductId(int ProductId);
        Task<ResultDto<List<ProductAttributeValueDto>>> GetProductAttributeValuesByProductAttributeId(int ProductAttributeId);
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

        public async Task<ResultDto<List<ProductAttributeDto>>> GetProductAttributesByProductId(int ProductId)
        {
            //get from db
            var productAttributes = await _mediator.Send(new GetProductAttributesByProductIdQuery(ProductId));
            if (productAttributes is null || !productAttributes.Any())
                return new ResultDto<List<ProductAttributeDto>>
                {
                    MessageEventType = Common.MessageEventTypes.MessageEventType.NotFound
                };

            return new ResultDto<List<ProductAttributeDto>>
            {
                IsSuccess = true,
                Data = productAttributes
            };
        }

        public async Task<ResultDto<List<ProductAttributeValueDto>>> GetProductAttributeValuesByProductAttributeId(int ProductAttributeId)
        {
            //get from db
            var productAttributeValues = await _mediator.Send(new GetProductAttributeValuesByProductAttributeIdQuery(ProductAttributeId));
            if (productAttributeValues is null || !productAttributeValues.Any())
                return new ResultDto<List<ProductAttributeValueDto>>
                {
                    MessageEventType = Common.MessageEventTypes.MessageEventType.NotFound
                };

            return new ResultDto<List<ProductAttributeValueDto>>
            {
                IsSuccess = true,
                Data = productAttributeValues
            };
        }
    }
    public class ProductAttributeDto
    {
        public int ProductAttributeId { get; set; }
        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
    }
    public class ProductAttributeValueDto
    {
        public int ProductAttributeValueId { get; set; }
        public string Value { get; set; }
    }
}
