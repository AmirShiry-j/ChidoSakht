using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.ProductVariant.Commands;
using Application.Store.AdminSection.RelatedProduct.Commands;
using Application.Store.AdminSection.RelatedProduct.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.RelatedProduct
{
    public interface IFacadeAdminRelatedProductService
    {
        IRelatedProductCommandsService RelatedProductCommandsService { get; }
        IRelatedProductQueriesService RelatedProductQueriesService { get; }
    }
    public class FacadeAdminRelatedProductService : IFacadeAdminRelatedProductService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminRelatedProductService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private IRelatedProductCommandsService _RelatedProductCommandsService;
        public IRelatedProductCommandsService RelatedProductCommandsService
        {
            get
            {
                return _RelatedProductCommandsService = _RelatedProductCommandsService ?? new RelatedProductCommandsService(_mediator, _localizationService);
            }
        }

        //Queries
        public IRelatedProductQueriesService _RelatedProductQueriesService;
        public IRelatedProductQueriesService RelatedProductQueriesService
        {
            get
            {
                return _RelatedProductQueriesService = _RelatedProductQueriesService ?? new RelatedProductQueriesService(_mediator, _localizationService);
            }
        }
    }
}
