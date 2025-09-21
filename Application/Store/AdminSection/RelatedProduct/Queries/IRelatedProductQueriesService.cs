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

namespace Application.Store.AdminSection.RelatedProduct.Queries
{
    public interface IRelatedProductQueriesService
    {
        Task<ResultDto<List<ProductDto>>> GetRelatedProductsAsync(int productId);
    }
    public class RelatedProductQueriesService : IRelatedProductQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public RelatedProductQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<List<ProductDto>>> GetRelatedProductsAsync(int productId)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(productId));
            if (product is null)
            {
                return new ResultDto<List<ProductDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            var result = await _mediator.Send(new GetRelatedProductsQuery(productId));

            return new ResultDto<List<ProductDto>>
            {
                IsSuccess = true,
                Data = result
            };
        }

    }
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
