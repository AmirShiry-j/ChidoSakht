using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Commons.Objects.Dtoes;
using Application.Store.AdminSection.CategoryService.Queries;
using AutoMapper;
using Domain.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CategoryService.Commands
{
    public interface ICreateCategoryService
    {
        Task<ResultDto<int>> Execute(CreateCategoryDto dto);
    }
    public class CreateCategoryService : ICreateCategoryService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public CreateCategoryService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<int>> Execute(CreateCategoryDto dto)
        {
            if (dto.ParentCategoryId is not null)
            {
                //Check Exist ParentCategory
                var category = await _mediator.Send(new GetCategoryByIdQuery((int)dto.ParentCategoryId));
                if (category is null)
                    return new ResultDto<int>
                    {
                        Message = _localizationService.GetMessageCategory(MessageKeysCategory.ParentCategoryNotFound.ToString())
                    };
            }

            //Create Category
            var command = new
                CreateCategoryCommand(dto.Name, dto.ParentCategoryId);
            var id = await _mediator.Send(command);

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = id
            };
        }
    }
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
