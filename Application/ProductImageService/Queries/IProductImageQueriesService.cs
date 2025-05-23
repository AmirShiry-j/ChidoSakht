using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductImageService.Queries
{
    public interface IProductImageQueriesService
    {
        Task<ResultDto<List<ProductImageDto>>> GetImages(int ProductId);
    }
    public class ProductImageQueriesService : IProductImageQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductImageQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<List<ProductImageDto>>> GetImages(int ProductId)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<List<ProductImageDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get images
            var images = await _mediator.Send(new GetProductImagesQueries(ProductId));

            return new ResultDto<List<ProductImageDto>>
            {
                IsSuccess = true,
                Data = images
            };
        }
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public string Url { get; set; }
    }
}
