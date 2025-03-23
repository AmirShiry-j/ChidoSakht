using Application.Common;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService.Queries
{
    public interface IGetCategoryInfoByIdService
    {
        Task<ResultDto<CategoryInfoDto>> Execute(int CategoryId);
    }
    public class GetCategoryInfoByIdService : IGetCategoryInfoByIdService
    {
        private readonly IMediator _mediator;
        public GetCategoryInfoByIdService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResultDto<CategoryInfoDto>> Execute(int CategoryId)
        {
            //Get category from db
            var categoryInfo = await _mediator.Send(new GetCategoryByIdQuery(CategoryId));

            //Check exist and return
            if (categoryInfo is not null)
            {
                return new ResultDto<CategoryInfoDto>
                {
                    IsSuccess = true,
                    Data = categoryInfo,
                };
            }
            else
            {
                return new ResultDto<CategoryInfoDto>
                {
                    IsSuccess = true,
                    Data = null,
                };
            }
        }
    }
    public class CategoryInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public List<Link> Links { get; set; }
    }
    public class GetCategoryByIdQuery : IRequest<CategoryInfoDto>
    {
        public int Id { get; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
