using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Store.UserSection.ProductService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.ProductService
{
    public interface IFacadeProductService
    {
        //Queries
        IProductQueriesService ProductQueriesService { get; }
    }
    public class FacadeProductService : IFacadeProductService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeProductService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
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
