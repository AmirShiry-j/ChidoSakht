using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.CategoryService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CategoryService
{
    public interface IFacadeCategoryService
    {
        //Queries
        IGetAllCategoriesAsTreeService GetAllCategoriesAsTreeService { get; }
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
        //Queries
        private IGetAllCategoriesAsTreeService _getAllCategoriesAsTreeService;
        public IGetAllCategoriesAsTreeService GetAllCategoriesAsTreeService
        {
            get
            {
                return _getAllCategoriesAsTreeService = _getAllCategoriesAsTreeService ?? new GetAllCategoriesAsTreeService(_mediator);
            }
        }
    }
}
