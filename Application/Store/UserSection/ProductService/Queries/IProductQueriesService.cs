using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using MediatR;

namespace Application.Store.UserSection.ProductService.Queries
{
    public interface IProductQueriesService
    {
        Task<ResultDto<ResultFilterDto>> GetProducts(ProductFilterForUserSectionDto filterDto);
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
            //var products = await _mediator.Send(new GetProductsByFilterQuery(filterDto));

            //return
            return new ResultDto<ResultFilterDto>
            {
                IsSuccess = true,
                Data = null
            };
        }
    }

    public class ProductFilterForUserSectionDto
    {
        public FilterFor FilterFor { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public bool? OnlyAvailableGoods { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
    public enum FilterFor
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
}
