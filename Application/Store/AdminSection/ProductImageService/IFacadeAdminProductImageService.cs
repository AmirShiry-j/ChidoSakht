using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.ProductImageService.Commands;
using Application.Store.AdminSection.ProductImageService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductImageService
{
    public interface IFacadeAdminProductImageService
    {
        //Commands
        IProductImageCommandsService ProductImageCommandsService { get; }
        IProductImageQueriesService ProductImageQueriesService { get; }
    }
    public class FacadeAdminProductImageService : IFacadeAdminProductImageService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminProductImageService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private IProductImageCommandsService _productImageCommandsService;
        public IProductImageCommandsService ProductImageCommandsService
        {
            get
            {
                return _productImageCommandsService = _productImageCommandsService ?? new ProductImageCommandsService(_mediator, _localizationService);
            }
        }

        //Queries
        private IProductImageQueriesService _productImageQueriesService;
        public IProductImageQueriesService ProductImageQueriesService
        {
            get
            {
                return _productImageQueriesService = _productImageQueriesService ?? new ProductImageQueriesService(_mediator, _localizationService);
            }
        }
    }
}
