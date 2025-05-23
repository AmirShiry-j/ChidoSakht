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
        Task<ResultDto<ProductDto>> GetOneProduct(int Id);

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
        public async Task<ResultDto<ProductDto>> GetOneProduct(int Id)
        {
            //get from db
            var product = await _mediator.Send(new GetProductByIdQuery((int)Id));

            //map to model
            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                LastUpdateTime = product.LastUpdateTime,
                CreateTime = product.CreateTime,
                Description = product.Description,
            };

            return new ResultDto<ProductDto>
            {
                IsSuccess = true,
                Data = dto
            };
        }
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public List<Link> Links { get; set; }
    }
}
