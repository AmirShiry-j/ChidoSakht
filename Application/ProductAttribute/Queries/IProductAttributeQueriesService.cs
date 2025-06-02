using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
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
        Task<ResultDto<List<ProductAttributeAndValuesDto>>> GetProductAttributesWithValuesByProductId(int ProductId);
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
            //check id in db
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<List<ProductAttributeDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var productAttributes = await _mediator.Send(new GetProductAttributesByProductIdQuery(ProductId));

            return new ResultDto<List<ProductAttributeDto>>
            {
                IsSuccess = true,
                Data = productAttributes
            };
        }

        

        public async Task<ResultDto<List<ProductAttributeValueDto>>> GetProductAttributeValuesByProductAttributeId(int ProductAttributeId)
        {
            //check id in db
            var productAttribute = await _mediator.Send(new GetProductAttributeByIdQuery(ProductAttributeId));
            if (productAttribute is null)
            {
                return new ResultDto<List<ProductAttributeValueDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var productAttributeValues = await _mediator.Send(new GetProductAttributeValuesByProductAttributeIdQuery(ProductAttributeId));

            return new ResultDto<List<ProductAttributeValueDto>>
            {
                IsSuccess = true,
                Data = productAttributeValues
            };
        }

        public async Task<ResultDto<List<ProductAttributeAndValuesDto>>> GetProductAttributesWithValuesByProductId(int ProductId)
        {
            //check id in db
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<List<ProductAttributeAndValuesDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var productAttributesAndValues = await _mediator.Send(new GetProductAttributesAndValuesByProductIdQuery(ProductId));

            return new ResultDto<List<ProductAttributeAndValuesDto>>
            {
                IsSuccess = true,
                Data = productAttributesAndValues
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

    public class ProductAttributeAndValuesDto
    {
        public int ProductAttributeId { get; set; }
        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
        public List<ProductAttributeValueDto> Values { get; set; }
    }
}
