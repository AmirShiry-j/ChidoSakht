using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService.Queries
{
    public interface IGetAllCategoriesAsTreeService
    {
        Task<ResultDto<List<BriefCategoryDto>>> Execute();
    }
    public class GetAllCategoriesAsTreeService : IGetAllCategoriesAsTreeService
    {
        private readonly IMediator _mediator;
        public GetAllCategoriesAsTreeService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResultDto<List<BriefCategoryDto>>> Execute()
        {
            //Get all categories from db
            var allCategories = await _mediator.Send(new GetAllCategoriesAsTreeQuery());

            //Check exist and return
            if (allCategories is not null)
            {
                return new ResultDto<List<BriefCategoryDto>>
                {
                    IsSuccess = true,
                    Data = allCategories,
                };
            }
            else
            {
                return new ResultDto<List<BriefCategoryDto>>
                {
                    IsSuccess = true,
                    Data = new List<BriefCategoryDto>(),
                };
            }
        }
    }

    public class BriefCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BriefCategoryDto> ChildCategories { get; set; }
    }
}
