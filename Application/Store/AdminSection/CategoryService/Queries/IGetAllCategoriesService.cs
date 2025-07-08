using Application.Commons.Objects.Dtoes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CategoryService.Queries
{
    public interface IGetAllCategoriesService
    {
        Task<ResultDto<List<BriefCategorySampleDto>>> Execute();
    }
    public class GetAllCategoriesService : IGetAllCategoriesService
    {
        private readonly IMediator _mediator;
        public GetAllCategoriesService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ResultDto<List<BriefCategorySampleDto>>> Execute()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());

            return new ResultDto<List<BriefCategorySampleDto>>
            {
                IsSuccess = true,
                Data = categories
            };
        }
    }

    public class BriefCategorySampleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
