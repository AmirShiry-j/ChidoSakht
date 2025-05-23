using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductImageService.Queries;
using Application.ProductService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductImageService.Commands
{
    public interface IProductImageCommandsService
    {
        Task<ResultDto<int>> CreateAImage(int ProductId, string Name);
        Task<ResultDto> DeleteAImage(string Name);
    }
    public class ProductImageCommandsService : IProductImageCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductImageCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<int>> CreateAImage(int ProductId, string Name)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<int>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Creaet image record
            var imageId = await _mediator.Send(new CreateProductImageCommand(ProductId, Name));

            return new ResultDto<int>
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Created,
                Data = imageId
            };
        }

        public async Task<ResultDto> DeleteAImage(string Name)
        {
            //check exist product
            var image = await _mediator.Send(new GetAProductImageByNameQuery(Name));
            if (image is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get image
            await _mediator.Send(new DeleteProductImageCommand(image));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

    }
}
