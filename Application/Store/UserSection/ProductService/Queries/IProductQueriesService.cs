using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductService.Queries;
using Domain.Products;
using MediatR;

namespace Application.Store.UserSection.ProductService.Queries
{
    public interface IProductQueriesService
    {
        Task<ResultDto<ResultFilterDto>> GetProducts(ProductFilterForUserSectionDto filterDto);
        Task<ResultDto<ProductWithDetailsDto>> GetOneProductWithDetails(int ProductId);
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

        public async Task<ResultDto<ResultFilterDto>> GetProducts(ProductFilterForUserSectionDto filterDto)
        {
            //get from db
            var products = await _mediator.Send(new GetProductsByFilterQuery(filterDto));

            //return
            return new ResultDto<ResultFilterDto>
            {
                IsSuccess = true,
                Data = products
            };
        }

        public async Task<ResultDto<ProductWithDetailsDto>> GetOneProductWithDetails(int ProductId)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(ProductId));
            if (product is null)
            {
                return new ResultDto<ProductWithDetailsDto>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var productWithDetails = await _mediator.Send(new GetOneProductWithDetailsByIdQuery(ProductId));

            //return
            return new ResultDto<ProductWithDetailsDto>
            {
                IsSuccess = true,
                Data = productWithDetails
            };
        }
    }

    public class ProductFilterForUserSectionDto
    {
        public TypeOrderByForProduct TypeOrderByForProduct { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public bool? OnlyAvailableGoods { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
    public enum TypeOrderByForProduct
    {
        Bazdid,
        Jadid,
        Forush,
        Arzan,
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UniqeLink { get; set; }
        public string? ImageAltText { get; set; }
        public string? NameIndexImage { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
    }
    public class ResultFilterDto
    {
        public int Page { get; set; }
        public int CountInPage { get; set; }
        public int CountAllPages { get; set; }
        public int CountAllItems { get; set; }
        public List<ProductDto> Products { get; set; }
    }

    public class ProductWithDetailsDto
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
        public List<ProductAttributeValueDto> AttributeAndValues { get; set; }
    }

    public class ProductAttributeAndValuesDto
    {
        public int ProductAttributeId { get; set; }
        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
        public bool UseForVariant { get; set; }
        public List<ProductAttributeValueDto> Values { get; set; }
    }
    public class ProductAttributeValueDto
    {
        public int ProductAttributeValueId { get; set; }
        public string Value { get; set; }
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
}
