using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductService.Queries;
using Application.ProductVariant.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant.Commands
{
    public interface IProductVariantCommandsService
    {
        Task<ResultDto<int>> CreateProductVariant(CreateProductVariantDto dto);
    }
    public class ProductVariantCommandsService : IProductVariantCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductVariantCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<int>> CreateProductVariant(CreateProductVariantDto dto)
        {
            //get from db
            var product = await _mediator.Send(new GetProductByIdQuery(dto.ProductId));
            if (product is null)
            {
                return new ResultDto<int>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //فعلا فرض میشه حداقل یه آیدی ارسال میکنه تا ولیدشنشو فعال کنم
            var validSendedValueIds = await _mediator.Send(new GetValidSendedProductAttributeValueIdsByProductIdQuery(dto.ProductId, dto.ProductAttributeValueIds));
            var inValidSendedValueIds = dto.ProductAttributeValueIds.Where(p => !validSendedValueIds.Contains(p)).ToList();
            if (inValidSendedValueIds.Any())
            {
                string strInValidIds = inValidSendedValueIds.Select(p => p.ToString()).Aggregate((p1, p2) => p1 + "," + p2).ToString();
                return new ResultDto<int>
                {
                    Message = strInValidIds,
                    MessageEventType = MessageEventType.BadRequest
                };
            }
            //همچنین باید مربوط به یک خاصیت نبودن آیدی هارم چک کرد
            //...

            //Create in db
            var productVariantId = await _mediator.Send(new CreateProductVariantCommand(dto));

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = productVariantId,
                MessageEventType = MessageEventType.Created
            };
        }
    }

    public class CreateProductVariantDto
    {
        public int ProductId { get; set; }
        public long Price { get; set; }
        public long SpecialPrice { get; set; }
        public int Stock { get; set; }
        public List<int> ProductAttributeValueIds { get; set; }
    }
}
