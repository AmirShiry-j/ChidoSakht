using Application.CategoryService.Queries;
using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CategoryService.Commands
{
    public interface IAddCategoryService
    {
        Task<ResultDto<int>> Execute(CreateCategoryDto dto);
    }
    public class AddCategoryService : IAddCategoryService
    {
        private readonly IMediator _mediator;
        public AddCategoryService(IMediator mediator)
        {
            _mediator = mediator;
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
                        Message = "کتگوری والدی با آیدی ارسال شده موجود نیست"
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
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; }
        public int? ParentCategoryId { get; }

        public CreateCategoryCommand(string Name, int? ParentCategoryId)
        {
            this.Name = Name;
            this.ParentCategoryId = ParentCategoryId;
        }
    }
}
