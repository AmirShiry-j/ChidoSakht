using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductImageService.Queries;
using Application.Store.AdminSection.ProductService.Commands;
using Application.Store.AdminSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductImageService.Commands
{
    public interface IProductImageCommandsService
    {
        Task<ResultDto<int>> CreateAImage(int ProductId, string Name);
        Task<ResultDto> DeleteAImage(string Name);
        Task<ResultDto> SetIndexImage(int ProductId, string Name);
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

            //Set publish if ...
            var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(image.ProductId));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

        public async Task<ResultDto> SetIndexImage(int ProductId, string Name)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check exist product
            var image = await _mediator.Send(new GetAProductImageByNameQuery(Name));
            if (image is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //set index
            image.IsIndex = true;
            product.NameIndexImage = image.Name;
            await _mediator.Send(new SetIndexImageForPrdouctCommand(image, product));

            //Set publish if ...
            var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(ProductId));

            return new ResultDto
            {
                IsSuccess = true,

            };
        }
    }
}
