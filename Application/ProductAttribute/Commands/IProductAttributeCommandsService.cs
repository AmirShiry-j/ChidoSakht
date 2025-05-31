using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute.Queries;
using Application.ProductService.Commands;
using Application.ProductService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.ProductAttribute.Commands
{
    public interface IProductAttributeCommandsService
    {
        Task<ResultDto<int>> Create(CreateProductAttributeDto dto);
        Task<ResultDto> Update(UpdateProductAttributeDto dto);
    }
    public class ProductAttributeCommandsService : IProductAttributeCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductAttributeCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<int>> Create(CreateProductAttributeDto dto)
        {
            //get from db
            var product = await _mediator.Send(new GetProductByIdQuery((int)dto.ProductId));
            if (product is null)
            {
                return new ResultDto<int>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Create
            var productAttributeId = await _mediator.Send(new CreateProductAttributeCommand(dto.ProductId, dto.Name, dto.AttributeType));

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = productAttributeId,
                MessageEventType = MessageEventType.Created
            };
        }

        public async Task<ResultDto> Update(UpdateProductAttributeDto dto)
        {
            //check id exist in db
            var productAttribute = await _mediator.Send(new GetProductAttributeByIdQuery((int)dto.ProductAttributeId));
            if (productAttribute is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //changes
            productAttribute.Name = dto.Name;

            //update
            await _mediator.Send(new UpdateProductAttributeCommand(productAttribute));

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }

    public class CreateProductAttributeDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
    }
    public class UpdateProductAttributeDto
    {
        public int ProductAttributeId { get; set; }
        public string Name { get; set; }
    }

}
