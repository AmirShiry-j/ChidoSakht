using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant.Queries
{
    public interface IProductVariantQueriesService
    {
        Task<ResultDto<List<ProductVariantDto>>> GetProductVariantsByProductId(int ProductId);
    }
    public class ProductVariantQueriesService : IProductVariantQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductVariantQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<List<ProductVariantDto>>> GetProductVariantsByProductId(int ProductId)
        {
            //get from db
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<List<ProductVariantDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Check Poduct was Variable
            if (product.ProductType != ProductType.Variable)
            {
                return new ResultDto<List<ProductVariantDto>>
                {
                    Message = "Temp-Mes  Product is not Variable",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Get data from db
            var productVariants = await _mediator.Send(new GetProductVariantsTypeVariableByProductIdQuery(ProductId));
            return new ResultDto<List<ProductVariantDto>>
            {
                Data = productVariants,
                IsSuccess = true
            };
        }
    }

    public class ProductVariantDto
    {
        public int ProductVariantId { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int Stock { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public ICollection<ProductVariantAttributeValueDto> ProductVariantAttributeValues { get; set; }
    }
    public class ProductVariantAttributeValueDto
    {
        public int ProductAttributeId { get; set; }
        public string ProductAttributeName { get; set; }
        public int ProductAttributeValueId { get; set; }
        public string ProductAttributeValue { get; set; }

    }
}
