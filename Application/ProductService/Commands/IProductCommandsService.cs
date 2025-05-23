using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.ConfigService;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.ProductService.Queries;
using Application.RoleService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.ProductService.Commands
{
    public interface IProductCommandsService
    {
        Task<ResultDto<int>> Upsert(int? ProductId, string Name);
        Task<ResultDto> SetDescription(int ProductId, string? Description);
        Task<ResultDto> SetUniqeLink(int ProductId, string? UniqeLink);
        Task<ResultDto> SetImageAltText(int ProductId, string? ImageAltText);

    }
    public class ProductCommandsService : IProductCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<int>> Upsert(int? ProductId, string Name)
        {

            if (ProductId is not null)
            {
                //get from db
                var product = await _mediator.Send(new GetProductByIdQuery((int)ProductId));
                if (product is null)
                {
                    return new ResultDto<int>
                    {
                        MessageEventType = MessageEventType.NotFound
                    };
                }

                //set new Name
                product.Name = Name;

                //Update in db
                await _mediator.Send(new UpdateProductCommand(product));

                return new ResultDto<int>
                {
                    IsSuccess = true,
                    MessageEventType = MessageEventType.Ok
                };
            }


            //Insert product
            var productId = await _mediator.Send(new InsertProductCommand(Name));
            //if (productId is null)
            //{
            //    return new ResultDto<int>
            //    {
            //        Message = _localizationService.GetMessageProduct(MessageKeysProduct.CreatedWasUnSuccess.ToString()),
            //    };
            //}

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = (int)productId,
                MessageEventType = MessageEventType.Created
            };
        }

        public async Task<ResultDto> SetDescription(int ProductId, string? Description)
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

            //set new value
            product.Description = Description;

            //Update in db
            await _mediator.Send(new UpdateProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }

        public async Task<ResultDto> SetUniqeLink(int ProductId, string? UniqeLink)
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

            if (UniqeLink is not null)
            {
                //is exist in db?
                var isValid = await _mediator.Send(new ValidateUniqeLinkQuery(ProductId, UniqeLink));
                if (isValid == false)
                {
                    return new ResultDto
                    {
                        MessageEventType = MessageEventType.BadRequest,
                        Message = _localizationService.GetMessageProduct(MessageKeysProduct.LinkNotUniqe.ToString())
                    };
                }
            }

            //set new value
            product.UniqeLink = UniqeLink;

            //Update in db
            await _mediator.Send(new UpdateProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }


        public async Task<ResultDto> SetImageAltText(int ProductId, string? ImageAltText)
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

            //set new value
            product.ImageAltText = ImageAltText;

            //Update in db
            await _mediator.Send(new UpdateProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }

    }
}
