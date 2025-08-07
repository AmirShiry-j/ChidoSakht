using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.ProductSpecificationService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.ProductSpecificationService
{
    public interface IFacadeProductSpecificationService
    {
        IProductSpecificationQueriesService ProductSpecificationQueriesService { get; }
    }
    public class FacadeProductSpecificationService : IFacadeProductSpecificationService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeProductSpecificationService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Queries
        private IProductSpecificationQueriesService _ProductSpecificationQueriesService;
        public IProductSpecificationQueriesService ProductSpecificationQueriesService
        {
            get
            {
                return _ProductSpecificationQueriesService = _ProductSpecificationQueriesService ?? new ProductSpecificationQueriesService(_mediator, _localizationService);
            }
        }
    }
}
