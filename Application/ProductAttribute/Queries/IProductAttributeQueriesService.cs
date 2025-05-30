using Application.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductAttribute.Queries
{
    public interface IProductAttributeQueriesService
    {
    }
    public class ProductAttributeQueriesService : IProductAttributeQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductAttributeQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
    }
}
