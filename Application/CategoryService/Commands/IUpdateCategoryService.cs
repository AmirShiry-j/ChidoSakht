using Application.CategoryService.Queries;
using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Domain.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService.Commands
{
    public interface IUpdateCategoryService
    {
        Task<ResultDto<int?>> Execute(UpdateCategoryDto Dto);
    }
    public class UpdateCategoryService : IUpdateCategoryService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public UpdateCategoryService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<int?>> Execute(UpdateCategoryDto Dto)
        {
            //Check Exist Category
            var category = await _mediator.Send(new GetCategoryByIdQuery(Dto.Id));
            if (category is null)
                return new ResultDto<int?>
                {
                    IsSuccess = true,
                    Data = null,//IsSuccess = true and Data = null mean record is not exist
                };

            if (Dto.ParentCategoryId is not null)
            {
                //Check Exist ParentCategory
                var parentCategory = await _mediator.Send(new GetCategoryByIdQuery((int)Dto.ParentCategoryId));
                if (parentCategory is null)
                    return new ResultDto<int?>
                    {
                        Message = _localizationService.GetMessageCategory(MessageKeysCategory.ParentCategoryNotFound.ToString())
                    };
            }

            //Update Category
            await _mediator.Send(new UpdateCategoryCommand(category));

            return new ResultDto<int?>
            {
                IsSuccess = true,
                Data = Dto.Id//mean exist
            };
        }
    }
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
