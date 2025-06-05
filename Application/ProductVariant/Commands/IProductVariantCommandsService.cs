using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute.Commands;
using Application.ProductAttribute.Queries;
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
        Task<ResultDto> DeleteProductVariant(int ProductVariantId);
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

            //Check Poduct wa Variable
            if (product.ProductType != ProductType.Variable)
            {
                return new ResultDto<int>
                {
                    Message = "Temp-Mes  Product is not Variable",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Validate ValueIds
            var validSendedValueIds = await _mediator.Send(new GetValidSendedProductAttributeValueIdsByProductIdQuery(dto.ProductId, dto.ProductAttributeValueIds));
            var inValidSendedValueIds = dto.ProductAttributeValueIds.Where(p => !validSendedValueIds.Contains(p)).ToList();
            if (inValidSendedValueIds.Any())
            {
                string strInValidIds = inValidSendedValueIds.Select(p => p.ToString()).Aggregate((p1, p2) => p1 + "," + p2).ToString();
                return new ResultDto<int>
                {
                    Message = "Temp-Mes ," + strInValidIds + " , این آیدی ها معتبر نیستند، یا آیدی ها پرتند یا متعلق به این محصول نیستند یا مربوط به خصوصیاتی نیستند که تیک قابلیت استفاده برای واریانتشون نخورده",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Validate AttributeId repetitive
            var groupBies = await _mediator.Send(new GroupbySendedProductAttributeValueIds_ByAttributeId_Query(dto.ProductAttributeValueIds));
            var attributeIdsAndTheirValueIds_CountMoreThanOne = groupBies.Where(p => p.ValueIds.Count() > 1).ToList();
            if (attributeIdsAndTheirValueIds_CountMoreThanOne.Any())
            {
                string Message = "Temp-Mes مقادیر زیر مربوط به یک خصوصیت هستند و نباید باشند";
                foreach (var attribute in attributeIdsAndTheirValueIds_CountMoreThanOne)
                {
                    Message += "\n";
                    Message += attribute.ProductAttributeId + " :  " + attribute.ValueIds.Select(p => p.ToString()).Aggregate((p1, p2) => p1 + "," + p2).ToString();
                }

                return new ResultDto<int>
                {
                    Message = Message,
                    MessageEventType = MessageEventType.BadRequest
                };
            }


            //Create in db
            var productVariantId = await _mediator.Send(new CreateProductVariantCommand(dto));

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = productVariantId,
                MessageEventType = MessageEventType.Created
            };
        }

        public async Task<ResultDto> DeleteProductVariant(int ProductVariantId)
        {
            //check id exist in db
            var productVariant = await _mediator.Send(new GetProductVariantByIdQuery(ProductVariantId));
            if (productVariant is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Check Variant was for ProductType Variable
            if (productVariant.ProductType != ProductType.Variable)
            {
                return new ResultDto
                {
                    Message = "Temp-Mes   Variant is not for a Variable Product",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Check Variant not use in any Sabad Kharid or Sefareshi
            var used = await _mediator.Send(new CheckUsedVariantInAnySefareshQuery(ProductVariantId));
            if (used)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.BadRequest,
                    Message = "Temp-Mes   واریانت در حداقل یک سفارش استفاده شده و نمیتوان آنرا حذف کرد"
                };
            }

            //Delete
            await _mediator.Send(new DeleteProductVariantCommand(productVariant));

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }

    public class CreateProductVariantDto
    {
        public int ProductId { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int Stock { get; set; }
        public List<int> ProductAttributeValueIds { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public ProductType ProductType { get; set; }

    }

    public class GroupBy_Values_By_AttributeId_Dto
    {
        public int ProductAttributeId { get; set; }
        public List<int> ValueIds { get; set; }
    }
}
