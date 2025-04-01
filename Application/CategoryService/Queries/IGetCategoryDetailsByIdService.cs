using Application.Common.Dtoes;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService.Queries
{
    public interface IGetCategoryDetailsByIdService
    {
        Task<ResultDto<CategoryDetailsDto>> Execute(int CategoryId);
    }
    public class GetCategoryDetailsByIdService : IGetCategoryDetailsByIdService
    {
        private readonly IMediator _mediator;
        public GetCategoryDetailsByIdService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResultDto<CategoryDetailsDto>> Execute(int CategoryId)
        {
            //Get category from db
            var categoryDetails = await _mediator.Send(new GetCategoryDetailsByIdQuery(CategoryId));

            //Check exist and return
            if (categoryDetails is not null)
            {
                return new ResultDto<CategoryDetailsDto>
                {
                    IsSuccess = true,
                    Data = categoryDetails,
                };
            }
            else
            {
                return new ResultDto<CategoryDetailsDto>
                {
                    IsSuccess = true,
                    Data = null,
                };
            }
        }
    }
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public List<ChildCategoryDto> ChildCategories { get; set; }
        public List<Link> Links { get; set; }
    }
    public class ChildCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChildCategoryDto> ChildCategories { get; set; }
    }
}
