using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductService.Queries
{
    public interface IProductQueriesService
    {
        Task<ResultDto<ProductDetailsDto>> GetOneProduct(int Id);
        Task<ResultDto<ResultSearchDto>> GetProducts(ProductFilterDto filterDto);
        Task<ResultDto<bool>> ValidateUniqeLink(string? UniqeLink);

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
            var product = await _mediator.Send(new GetProductByIdQuery((int)Id));
            if (product is null)
                return new ResultDto<ProductDetailsDto>
                {
                    MessageEventType = Common.MessageEventTypes.MessageEventType.NotFound
                };

            //map to model
            var dto = new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                LastUpdateTime = product.LastUpdateTime,
                CreateTime = product.CreateTime,
                Description = product.Description,
            };

            return new ResultDto<ProductDetailsDto>
            {
                IsSuccess = true,
                Data = dto
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

        public async Task<ResultDto<bool>> ValidateUniqeLink(string UniqeLink)
        {
            //is exist in db?
            var isValid = await _mediator.Send(new ValidateUniqeLinkQuery(UniqeLink));

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

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public List<Link> Links { get; set; }
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
