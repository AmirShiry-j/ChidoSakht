using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductService.Queries;
using Application.Store.AdminSection.ProductSpecificationService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecificationService.Commands
{
    public interface IProductSpecificationCommandsService
    {
        // Groups
        Task<ResultDto<long>> CreateSpecGroupAsync(int productId, string title);
        Task<ResultDto> UpdateSpecGroupAsync(long groupId, string title);
        Task<ResultDto> DeleteSpecGroupAsync(long groupId);

        // Specs
        Task<ResultDto<long>> CreateSpecAsync(long groupId, string key, string value);
        Task<ResultDto> UpdateSpecAsync(long specId, string key, string value);
        Task<ResultDto> DeleteSpecAsync(long specId);
    }

    public class ProductSpecificationCommandsService : IProductSpecificationCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductSpecificationCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<long>> CreateSpecGroupAsync(int productId, string title)
        {
            //get from db
            var product = await _mediator.Send(new GetProductByIdQuery(productId));
            if (product is null)
            {
                return new ResultDto<long>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            var id = await _mediator.Send(new CreateSpecGroupCommand(productId, title));

            return new ResultDto<long>
            {
                IsSuccess = true,
                Data = id,
                MessageEventType = MessageEventType.Created
            };
        }

        public async Task<ResultDto> UpdateSpecGroupAsync(long groupId, string title)
        {
            //get from db
            var specGroup = await _mediator.Send(new GetSpecGroupByIdQuery(groupId));
            if (specGroup is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //changes
            specGroup.Title = title;

            //update
            _mediator.Send(new UpdateSpecGroupCommand(specGroup));

            return new ResultDto
            {
                IsSuccess = true
            };
        }

        public async Task<ResultDto> DeleteSpecGroupAsync(long groupId)
        {
            //get from db
            var specGroup = await _mediator.Send(new GetSpecGroupByIdQuery(groupId));
            if (specGroup is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            _mediator.Send(new DeleteSpecGroupCommand(specGroup));

            return new ResultDto
            {
                IsSuccess = true
            };
        }

        public async Task<ResultDto<long>> CreateSpecAsync(long groupId, string key, string value)
        {
            //get from db
            var specGroup = await _mediator.Send(new GetSpecGroupByIdQuery(groupId));
            if (specGroup is null)
            {
                return new ResultDto<long>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //create
            var id = await _mediator.Send(new CreateSpecCommand(groupId, key, value));

            return new ResultDto<long>
            {
                IsSuccess = true,
                Data = id,
                MessageEventType = MessageEventType.Created
            };
        }

        public async Task<ResultDto> UpdateSpecAsync(long specId, string key, string value)
        {
            //get from db
            var spec = await _mediator.Send(new GetSpecByIdQuery(specId));
            if (spec is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //change
            spec.Key = key;
            spec.Value = value;

            //update
            _mediator.Send(new UpdateSpecCommand(spec));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

        public async Task<ResultDto> DeleteSpecAsync(long specId)
        {
            //get from db
            var spec = await _mediator.Send(new GetSpecByIdQuery(specId));
            if (spec is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //delete
            _mediator.Send(new DeleteSpecCommand(spec));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }
    }
}
