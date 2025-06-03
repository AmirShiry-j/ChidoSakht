using Application.CategoryService.Queries;
using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.ConfigService;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.ProductService.Queries;
using Application.ProductVariant.Commands;
using Application.ProductVariant.Queries;
using Application.RoleService.Queries;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.ProductService.Commands
{
    public interface IProductCommandsService
    {
        Task<ResultDto<int>> Upsert(int? ProductId, string Name, ProductType ProductType);
        Task<ResultDto> UpdateInfoProductSample(UpdateInfoProductSampleDto dto);
        Task<ResultDto> SetDescription(int ProductId, string? Description);
        Task<ResultDto> SetUniqeLink(int ProductId, string? UniqeLink);
        Task<ResultDto> SetImageAltText(int ProductId, string? ImageAltText);
        Task<ResultDto> SetUniCode(int ProductId, string? UniCode);
        Task<ResultDto> SetCategoryId(int ProductId, int? CategoryId);

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
        public async Task<ResultDto<int>> Upsert(int? ProductId, string Name, ProductType ProductType)
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

                if (product.ProductType == ProductType.Variable && ProductType == ProductType.Sample)
                {
                    ////Check product does not have any ProductVariant
                    //var productVariantsTypeVariables = (await _mediator.Send(new GetProductVariantsTypeVariableByProductIdQuery((int)ProductId)));
                    //if (productVariantsTypeVariables.Any())
                    //{
                    //    return new ResultDto<int>
                    //    {
                    //        Message = "Temp-Mes  این محصول متغیر دارای واریانت هست . ابتدا باید آنهارا حذف کنید. تا بتوانید آن را به محصول ساده تغییر دهید",
                    //        MessageEventType = MessageEventType.BadRequest
                    //    };
                    //}

                    return new ResultDto<int>
                    {
                        Message = "Temp-Mes نمیتوان نوع محصول را تغییر داد",
                        MessageEventType = MessageEventType.BadRequest
                    };
                }
                else if (product.ProductType == ProductType.Sample && ProductType == ProductType.Variable)
                {
                    ////Check product does not have any ProductVariant
                    //var productVariantTypeSample = (await _mediator.Send(new GetProductVariantTypeSampleByProductIdQuery((int)ProductId)));
                    //if (productVariantTypeSample is not null)
                    //{
                    //    //check چک کردن اینکه از این محصول خریدی ثبت نشده باشه
                    //    //..
                    //    //.. برای بعدن
                    //    //.. WarCheckWar

                    //    var productVariant = (await _mediator.Send(new GetProductVariantByIdQuery(productVariantTypeSample.ProductVariantId)));
                    //    await _mediator.Send(new DeleteProductVariantCommand(productVariant));
                    //}

                    return new ResultDto<int>
                    {
                        Message = "Temp-Mes نمیتوان نوع محصول را تغییر داد",
                        MessageEventType = MessageEventType.BadRequest
                    };
                }

                //set new Name
                product.Name = Name;
                product.ProductType = ProductType;

                //Update in db
                await _mediator.Send(new UpdateProductCommand(product));

                return new ResultDto<int>
                {
                    IsSuccess = true,
                    MessageEventType = MessageEventType.Ok
                };
            }


            //Insert product
            var productId = await _mediator.Send(new InsertProductCommand(Name, ProductType));
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

        public async Task<ResultDto> SetUniCode(int ProductId, string? UniCode)
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

            if (UniCode is not null)
            {
                //is exist in db?
                var isValid = await _mediator.Send(new ValidateUniCodeQuery(ProductId, UniCode));
                if (isValid == false)
                {
                    return new ResultDto
                    {
                        MessageEventType = MessageEventType.BadRequest,
                        Message = _localizationService.GetMessageProduct(MessageKeysProduct.UniCodeNotUniqe.ToString())
                    };
                }
            }

            //set new value
            product.UniCode = UniCode;

            //Update in db
            await _mediator.Send(new UpdateProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }

        public async Task<ResultDto> SetCategoryId(int ProductId, int? CategoryId)
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

            if (CategoryId is null)
            {
                //change
                product.CategoryId = null;
                await _mediator.Send(new UpdateProductCommand(product));

                return new ResultDto
                {
                    IsSuccess = true
                };
            }

            //with data

            //check exist
            var category = await _mediator.Send(new GetCategoryByIdQuery((int)CategoryId));
            if (category is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //change
            product.CategoryId = (int)CategoryId;
            await _mediator.Send(new UpdateProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true
            };
        }

        public async Task<ResultDto> UpdateInfoProductSample(UpdateInfoProductSampleDto dto)
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

            //check product was sample
            if (product.ProductType != ProductType.Sample)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.BadRequest,
                    Message = "Temp-Mes  product is not Sample"
                };
            }

            //map dto to domain
            var productVariantDto = (await _mediator.Send(new GetProductVariantTypeSampleByProductIdQuery(dto.ProductId)));
            if (productVariantDto is null)
            {
                var newProductVariant = new CreateProductVariantDto
                {
                    Price = dto.Price,
                    SpecialPrice = dto.SpecialPrice,
                    Stock = dto.Stock,
                    ProductId = dto.ProductId,
                    Height = dto.Height,
                    Length = dto.Length,
                    Weight = dto.Weight,
                    Width = dto.Width,
                    ProductType = ProductType.Sample
                };

                var productVariantId = await _mediator.Send(new CreateProductVariantCommand(newProductVariant));
            }
            else
            {
                //get main domain
                var productVariant = await _mediator.Send(new GetProductVariantByIdQuery(productVariantDto.ProductVariantId));
                var productVariantTransportation = await _mediator.Send(new GetProductVariantTransportationByProductVariantIdQuery(productVariantDto.ProductVariantId));

                //set new Change
                productVariantTransportation.Width = dto.Width;
                productVariantTransportation.Weight = dto.Weight;
                productVariantTransportation.Length = dto.Length;
                productVariantTransportation.Height = dto.Height;

                productVariant.ProductId = dto.ProductId;
                productVariant.Price = dto.Price;
                productVariant.SpecialPrice = dto.SpecialPrice;
                productVariant.Stock = dto.Stock;
                productVariant.ProductVariantTransportation = productVariantTransportation;
                //productVariant.ProductType = ProductType.Sample; //No need

                await _mediator.Send(new UpdateProductVariantCommand(productVariant));
            }

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }
    }


    public class UpdateInfoProductSampleDto
    {
        public int ProductId { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int Stock { get; set; }
        //
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
    }
}
