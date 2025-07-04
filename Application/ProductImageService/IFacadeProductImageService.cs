using Application.Interfaces.Localization;
using Application.ProductImageService.Commands;
using Application.ProductImageService.Queries;
using Application.ProductService.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductImageService
{
    public interface IFacadeProductImageService
    {
        //Commands
        IProductImageCommandsService ProductImageCommandsService { get; }
        IProductImageQueriesService ProductImageQueriesService { get; }
    }
    public class FacadeProductImageService : IFacadeProductImageService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeProductImageService(IMediator mediator, ILocalizationService localizationService)
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
