using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductService.Queries;
using Application.Store.AdminSection.RelatedProduct.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.RelatedProduct.Commands
{
    public interface IRelatedProductCommandsService
    {
        Task<ResultDto> AddRelatedProductsAsync(AddRelatedProductsDto dto);
        Task<ResultDto> RemoveRelatedProductsAsync(RemoveRelatedProductDto dto);
    }
    public class RelatedProductCommandsService : IRelatedProductCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public RelatedProductCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto> AddRelatedProductsAsync(AddRelatedProductsDto dto)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(dto.ProductId));
            if (product is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            var productId = dto.ProductId;
            var relatedIds = dto.RelatedProductIds
                                .Where(id => id != productId)
                                .Distinct()
                                .ToList();

            // لیست روابطی که قبلاً در دیتابیس وجود دارن (در هر جهت)
            var existingRelations = await _mediator.Send(
                new GetExistingRelationsQuery(productId, relatedIds));

            // شناسایی فقط آیدی‌هایی که قبلاً ثبت نشده‌اند
            var alreadyRelatedIds = existingRelations
                .Select(r => r.ProductId == productId ? r.RelatedProductId : r.ProductId)
                .ToHashSet();

            var newIds = relatedIds
                .Where(id => !alreadyRelatedIds.Contains(id))
                .ToList();

            foreach (var relatedId in newIds)
            {
                await _mediator.Send(new AddRelatedProductCommand(productId, relatedId));
                await _mediator.Send(new AddRelatedProductCommand(relatedId, productId));
            }

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = Commons.Objects.MessageEventTypes.MessageEventType.Created
            };
        }


        public async Task<ResultDto> RemoveRelatedProductsAsync(RemoveRelatedProductDto dto)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(dto.ProductId));
            if (product is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            var productId = dto.ProductId;
            var relatedIds = dto.RelatedProductIds
                .Where(id => id != productId)
                .Distinct();

            foreach (var relatedId in relatedIds)
            {
                await _mediator.Send(new RemoveRelatedProductCommand(productId, relatedId));
                await _mediator.Send(new RemoveRelatedProductCommand(relatedId, productId));
            }

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = Commons.Objects.MessageEventTypes.MessageEventType.Ok
            };
        }
    }

    public class AddRelatedProductsDto
    {
        public int ProductId { get; set; }
        public List<int> RelatedProductIds { get; set; }
    }

    public class RemoveRelatedProductDto
    {
        public int ProductId { get; set; }
        public List<int> RelatedProductIds { get; set; }
    }

}
