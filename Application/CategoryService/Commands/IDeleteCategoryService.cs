using Application.CategoryService.Queries;
using Application.Common;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService.Commands
{
    public interface IDeleteCategoryService
    {
        Task<ResultDto> Execute(int CategoryId);
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
        public async Task<ResultDto> Execute(int CategoryId)
        {
            //Check Exist Category
            var category = await _mediator.Send(new GetCategoryByIdQuery(CategoryId));
            if (category is null)
                return new ResultDto
                {
                    Message = _localizationService.GetMessageCategory(MessageKeysCategory.CategoryIdNotFound.ToString())
                };

            //Check cateogory has any products
            //...

            //Delete Category
            await _mediator.Send(new DeleteCategoryCommand(CategoryId));

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
