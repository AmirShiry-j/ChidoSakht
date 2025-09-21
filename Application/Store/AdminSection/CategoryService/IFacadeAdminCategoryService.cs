using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.CategoryService.Commands;
using Application.Store.AdminSection.CategoryService.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CategoryService
{
    public interface IFacadeAdminCategoryService
    {
        //Commands
        ICreateCategoryService CreateCategoryService { get; }
        IUpdateCategoryService UpdateCategoryService { get; }
        IDeleteCategoryService DeleteCategoryService { get; }
        //Queries
        IGetCategoryDetailsByIdService GetCategoryDetailsByIdService { get; }
        IGetAllCategoriesAsTreeService GetAllCategoriesAsTreeService { get; }
        IGetAllCategoriesService GetAllCategoriesService { get; }
    }
    public class FacadeAdminCategoryService : IFacadeAdminCategoryService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminCategoryService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        //Commands
        #region Commands
        //Create
        private ICreateCategoryService _createCategoryService;
        public ICreateCategoryService CreateCategoryService
        {
            get
            {
                return _createCategoryService = _createCategoryService ?? new CreateCategoryService(_mediator, _localizationService);
            }
        }
        //Update
        private IUpdateCategoryService _updateCategoryService;
        public IUpdateCategoryService UpdateCategoryService
        {
            get
            {
                return _updateCategoryService = _updateCategoryService ?? new UpdateCategoryService(_mediator, _localizationService);
            }
        }
        //Delete
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
        private IGetCategoryDetailsByIdService _getCategoryDetailsByIdService;
        public IGetCategoryDetailsByIdService GetCategoryDetailsByIdService
        {
            get
            {
                return _getCategoryDetailsByIdService = _getCategoryDetailsByIdService ?? new GetCategoryDetailsByIdService(_mediator);
            }
        }
        //
        private IGetAllCategoriesAsTreeService _getAllCategoriesAsTreeService;
        public IGetAllCategoriesAsTreeService GetAllCategoriesAsTreeService
        {
            get
            {
                return _getAllCategoriesAsTreeService = _getAllCategoriesAsTreeService ?? new GetAllCategoriesAsTreeService(_mediator);
            }
        }
        //
        private IGetAllCategoriesService _GetAllCategoriesService;
        public IGetAllCategoriesService GetAllCategoriesService
        {
            get
            {
                return _GetAllCategoriesService = _GetAllCategoriesService ?? new GetAllCategoriesService(_mediator);
            }
        }
        #endregion
    }
}
