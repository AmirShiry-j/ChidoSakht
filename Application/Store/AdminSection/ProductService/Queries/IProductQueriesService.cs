using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductService.Queries
{
    public interface IProductQueriesService
    {
        Task<ResultDto<ProductDetailsDto>> GetOneProduct(int Id);
        Task<ResultDto<ResultSearchDto>> GetProducts(ProductFilterDto filterDto);
        Task<ResultDto<bool>> ValidateUniqeLink(int ProductId, string? UniqeLink);
        Task<ResultDto<bool>> ValidateUniCode(int ProductId, string? UniCode);
        Task<ResultDto<bool>> GetPublicationStatus(int ProductId);

    }
    public class ProductQueriesService : IProductQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<ProductDetailsDto>> GetOneProduct(int Id)
        {
            //get from db
            var productDto = await _mediator.Send(new GetProductDetailsByIdQuery(Id));
            if (productDto is null)
                return new ResultDto<ProductDetailsDto>
                {
                    MessageEventType = MessageEventType.NotFound
                };

            return new ResultDto<ProductDetailsDto>
            {
                IsSuccess = true,
                Data = productDto
            };
        }

        public async Task<ResultDto<ResultSearchDto>> GetProducts(ProductFilterDto filterDto)
        {
            //get from db
            var products = await _mediator.Send(new GetProductsByFilterQuery(filterDto));

            //return
            return new ResultDto<ResultSearchDto>
            {
                IsSuccess = true,
                Data = products
            };
        }

        public async Task<ResultDto<bool>> GetPublicationStatus(int ProductId)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<bool>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            return new ResultDto<bool>
            {
                IsSuccess = true,
                Data = product.IsPublished
            };
        }

        public async Task<ResultDto<bool>> ValidateUniCode(int ProductId, string? UniCode)
        {
            //is exist in db?
            var isValid = await _mediator.Send(new ValidateUniCodeQuery(ProductId, UniCode));

            //return
            return new ResultDto<bool>
            {
                IsSuccess = true,
                Data = isValid
            };
        }

        public async Task<ResultDto<bool>> ValidateUniqeLink(int ProductId, string UniqeLink)
        {
            //is exist in db?
            var isValid = await _mediator.Send(new ValidateUniqeLinkQuery(ProductId, UniqeLink));

            //return
            return new ResultDto<bool>
            {
                IsSuccess = true,
                Data = isValid
            };
        }
    }
    public class ProductFilterDto
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public string? Description { get; set; }
        public string? UniqeLink { get; set; }
        public string? ImageAltText { get; set; }
        public string? NameIndexImage { get; set; }
        public string? UrlNameIndexImage { get; set; }
        public string? UniCode { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsPublished { get; set; }
        public InfoForSampleProductDto InfoForSampleProduct { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public List<Link> Links { get; set; }
    }
    public class InfoForSampleProductDto
    {

        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int Stock { get; set; }
        //
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public string? NameIndexImage { get; set; }
        public string? UrlNameIndexImage { get; set; }
        public Link Link { get; set; }

    }
    public class ResultSearchDto
    {
        public int Page { get; set; }
        public int CountInPage { get; set; }
        public int CountAllPages { get; set; }
        public int CountAllItems { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
