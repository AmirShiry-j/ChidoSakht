using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.ProductService.Commands;
using Application.Store.AdminSection.ProductService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductService
{
    public interface IFacadeAdminProductService
    {
        //Commands
        IProductCommandsService ProductCommandsService { get; }
        //Queries
        IProductQueriesService ProductQueriesService { get; }
    }

    public class FacadeAdminProductService : IFacadeAdminProductService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminProductService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private IProductCommandsService _productCommandsService;
        public IProductCommandsService ProductCommandsService
        {
            get
            {
                return _productCommandsService = _productCommandsService ?? new ProductCommandsService(_mediator, _localizationService);
            }
        }

        //Queries
        private IProductQueriesService _productQueriesService;
        public IProductQueriesService ProductQueriesService
        {
            get
            {
                return _productQueriesService = _productQueriesService ?? new ProductQueriesService(_mediator, _localizationService);
            }
        }
    }
}
