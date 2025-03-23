using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Application.Interfaces.Contexts;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService
{
    public interface IFacadeCategoryService
    {
        //Commands
        IAddCategoryService AddCategoryService { get; }
        //Queries
        IGetCategoryInfoByIdService GetCategoryInfoByIdService { get; }
    }
    public class FacadeCategoryService : IFacadeCategoryService
    {
        private readonly IMediator _mediator;
        public FacadeCategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }
        //Commands
        #region Commands
        private IAddCategoryService _addCategoryService;
        public IAddCategoryService AddCategoryService
        {
            get
            {
                return _addCategoryService = _addCategoryService ?? new AddCategoryService(_mediator);
            }
        }
        #endregion

        //Queries
        #region Queries
        private IGetCategoryInfoByIdService _getCategoryInfoByIdService;
        public IGetCategoryInfoByIdService GetCategoryInfoByIdService
        {
            get
            {
                return _getCategoryInfoByIdService = _getCategoryInfoByIdService ?? new GetCategoryInfoByIdService(_mediator);
            }
        }
        #endregion
    }
}
