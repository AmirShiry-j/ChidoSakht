using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.ProductAttribute.Commands;
using Application.Store.AdminSection.ProductAttribute.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductAttribute
{
    public interface IFacadeProductAttributeService
    {
        IProductAttributeCommandsService ProductAttributeCommandsService { get; }
        IProductAttributeQueriesService ProductAttributeQueriesService { get; }
    }
    public class FacadeProductAttributeService : IFacadeProductAttributeService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeProductAttributeService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private IProductAttributeCommandsService _productAttributeCommandsService;
        public IProductAttributeCommandsService ProductAttributeCommandsService
        {
            get
            {
                return _productAttributeCommandsService = _productAttributeCommandsService ?? new ProductAttributeCommandsService(_mediator, _localizationService);
            }
        }

        //Queries
        private IProductAttributeQueriesService _productAttributeQueriesService;
        public IProductAttributeQueriesService ProductAttributeQueriesService
        {
            get
            {
                return _productAttributeQueriesService = _productAttributeQueriesService ?? new ProductAttributeQueriesService(_mediator, _localizationService);
            }
        }
    }
}
