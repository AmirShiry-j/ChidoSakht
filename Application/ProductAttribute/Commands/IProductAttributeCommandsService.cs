using Application.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductAttribute.Commands
{
    public interface IProductAttributeCommandsService
    {
    }
    public class ProductAttributeCommandsService : IProductAttributeCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductAttributeCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
    }
}
