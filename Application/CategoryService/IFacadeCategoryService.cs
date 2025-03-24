using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Application.Interfaces.Contexts;
using Application.Interfaces.Localization;
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
        IDeleteCategoryService DeleteCategoryService { get; }
        //Queries
        IGetCategoryInfoByIdService GetCategoryInfoByIdService { get; }
    }
    public class FacadeCategoryService : IFacadeCategoryService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeCategoryService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        //Commands
        #region Commands
        private IAddCategoryService _addCategoryService;
        public IAddCategoryService AddCategoryService
        {
            get
            {
                return _addCategoryService = _addCategoryService ?? new AddCategoryService(_mediator, _localizationService);
            }
        }
        private IDeleteCategoryService _deleteCategoryService;
        public IDeleteCategoryService DeleteCategoryService
        {
            get
            {
                return _deleteCategoryService = _deleteCategoryService ?? new DeleteCategoryService(_mediator, _localizationService);
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
