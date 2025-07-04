using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Commons.Objects.Dtoes;
using Application.Store.AdminSection.CategoryService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CategoryService.Commands
{
    public interface IDeleteCategoryService
    {
        Task<ResultDto<int?>> Execute(int CategoryId);
    }
    public class DeleteCategoryService : IDeleteCategoryService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public DeleteCategoryService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<int?>> Execute(int CategoryId)
        {
            //Check Exist Category
            var category = await _mediator.Send(new GetCategoryByIdQuery(CategoryId));
            if (category is null)
                return new ResultDto<int?>
                {
                    IsSuccess = true,
                    Data = null,//IsSuccess = true and Data = null mean record is not exist
                };

            //Check cateogory has any Children
            var checkCategoryHasAnyChildren = await _mediator.Send(new CheckCategoryHasAnyChildrenByIdQuery(CategoryId));
            if (checkCategoryHasAnyChildren)
                return new ResultDto<int?>
                {
                    Message = _localizationService.GetMessageCategory(MessageKeysCategory.ForDelete_CategoryHasChilren.ToString())
                };

            //Check cateogory has any products
            //...

            //Delete Category
            await _mediator.Send(new DeleteCategoryCommand(category));

            return new ResultDto<int?>
            {
                IsSuccess = true,
                Data = CategoryId//mean exist
            };
        }
    }
}
