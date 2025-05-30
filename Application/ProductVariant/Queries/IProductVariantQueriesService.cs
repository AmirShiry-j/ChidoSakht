using Application.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant.Queries
{
    public interface IProductVariantQueriesService
    {
    }
    public class ProductVariantQueriesService : IProductVariantQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductVariantQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
    }
}
