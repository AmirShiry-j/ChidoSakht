using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.CategoryService.Queries;
using Application.Store.AdminSection.ProductImageService.Commands;
using Application.Store.AdminSection.ProductService.Queries;
using Application.Store.AdminSection.ProductVariant.Commands;
using Application.Store.AdminSection.ProductVariant.Queries;
using Application.Store.AdminSection.RelatedProduct.Commands;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Store.AdminSection.ProductService.Commands
{
    public interface IProductCommandsService
    {
        Task<ResultDto<int>> Create(string Name, ProductType ProductType);
        Task<ResultDto> UpdateInfoProductSample(UpdateInfoProductSampleDto dto);
        Task<ResultDto> SetName(int ProductId, string Name);
        Task<ResultDto> SetDescription(int ProductId, string? Description);
        Task<ResultDto> SetUniqeLink(int ProductId, string? UniqeLink);
        Task<ResultDto> SetImageAltText(int ProductId, string? ImageAltText);
        Task<ResultDto> SetUniCode(int ProductId, string? UniCode);
        Task<ResultDto> SetCategoryId(int ProductId, int? CategoryId);
        Task<ResultDto> DeleteAProduct(int ProductId, string BasePathImages);
        Task<ResultDto> PublishProductIfValidate(int ProductId);

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
        public async Task<ResultDto<int>> Create(string Name, ProductType ProductType)
        {
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

        public async Task<ResultDto> SetName(int ProductId, string Name)
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
            product.Name = Name;

            //Update in db
            await _mediator.Send(new UpdateProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
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

            //Set publish if ...
            var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(ProductId));

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

                ////Set publish if ...
                //var resultPublish1 = await _mediator.Send(new PublishProductIfValidateCommand(ProductId));

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

            ////Set publish if ...
            //var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(ProductId));

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
            var productVariantDto = await _mediator.Send(new GetProductVariantTypeSampleByProductIdQuery(dto.ProductId));
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

            //Set publish if ...
            var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(dto.ProductId));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }

        public async Task<ResultDto> DeleteAProduct(int ProductId, string BasePathImages)
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

            //Check product dont used in any sefaresh
            var used = await _mediator.Send(new CheckUsedProductInAnySefareshQuery(ProductId));
            if (used)
                return new ResultDto
                {
                    MessageEventType = MessageEventType.BadRequest,
                    Message = "Temp-Mes    محصول در حداقل یک سفارش استفاده شده و نمیتوان آنرا حذف کرد"
                };

            //Delete Images
            await _mediator.Send(new DeleteProductImagesByProductIdCommand(ProductId, BasePathImages));

            //Delete Relateds
            await _mediator.Send(new DeleteRelatedProductsByProductIdCommand(ProductId));

            //Set publish if ...
            var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(ProductId));

            //Delete product
            await _mediator.Send(new DeleteProductCommand(product));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }

        public async Task<ResultDto> PublishProductIfValidate(int ProductId)
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

            //Set publish if ...
            var resultPublish = await _mediator.Send(new PublishProductIfValidateCommand(ProductId));

            if (resultPublish.Item1)
            {
                return new ResultDto
                {
                    IsSuccess = true,
                };
            }
            else
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    MessageEventType = MessageEventType.BadRequest,
                    Message = resultPublish.Item2
                };
            }
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
