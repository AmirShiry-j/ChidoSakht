using Application.Interfaces.Localization;
using Application.ProductVariant.Commands;
using Application.ProductVariant.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant
{
    public interface IFacadeProductVariantService
    {
        IProductVariantCommandsService ProductVariantCommandsService { get; }
        IProductVariantQueriesService ProductVariantQueriesService { get; }
    }
    public class FacadeProductVariantService : IFacadeProductVariantService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeProductVariantService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private IProductVariantCommandsService _productVariantCommandsService;
        public IProductVariantCommandsService ProductVariantCommandsService
        {
            get
            {
                return _productVariantCommandsService = _productVariantCommandsService ?? new ProductVariantCommandsService(_mediator, _localizationService);
            }
        }

        //Queries
        private IProductVariantQueriesService _productVariantQueriesService;
        public IProductVariantQueriesService ProductVariantQueriesService
        {
            get
            {
                return _productVariantQueriesService = _productVariantQueriesService ?? new ProductVariantQueriesService(_mediator, _localizationService);
            }
        }
    }
}
