using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.ProductAttribute.Commands;
using Application.Store.AdminSection.ProductAttribute.Queries;
using Application.Store.AdminSection.ProductSpecificationService.Commands;
using Application.Store.AdminSection.ProductSpecificationService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecificationService
{
    public interface IFacadeAdminProductSpecificationService
    {
        IProductSpecificationCommandsService ProductSpecificationCommandsService { get; }
        IProductSpecificationQueriesService ProductSpecificationQueriesService { get; }
    }
    public class FacadeAdminProductSpecificationService : IFacadeAdminProductSpecificationService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminProductSpecificationService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private IProductSpecificationCommandsService _ProductSpecificationCommandsService;
        public IProductSpecificationCommandsService ProductSpecificationCommandsService
        {
            get
            {
                return _ProductSpecificationCommandsService = _ProductSpecificationCommandsService ?? new ProductSpecificationCommandsService(_mediator, _localizationService);
            }
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
