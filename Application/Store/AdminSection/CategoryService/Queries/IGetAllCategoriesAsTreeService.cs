using Application.Commons.Objects.Dtoes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CategoryService.Queries
{
    public interface IGetAllCategoriesAsTreeService
    {
        Task<ResultDto<AllCategoryDto>> Execute();
    }
    public class GetAllCategoriesAsTreeService : IGetAllCategoriesAsTreeService
    {
        private readonly IMediator _mediator;
        public GetAllCategoriesAsTreeService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResultDto<AllCategoryDto>> Execute()
        {
            //Get all categories from db
            var allCategories = await _mediator.Send(new GetAllCategoriesAsTreeQuery());

            return new ResultDto<AllCategoryDto>
            {
                IsSuccess = true,
                Data = new AllCategoryDto
                {
                    AllCategories = allCategories
                },
            };
        }
    }

    public class BriefCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BriefCategoryDto> ChildCategories { get; set; }
    }
    public class AllCategoryDto
    {
        public List<BriefCategoryDto> AllCategories { get; set; }
        public List<Link> Links { get; set; }
    }
}
