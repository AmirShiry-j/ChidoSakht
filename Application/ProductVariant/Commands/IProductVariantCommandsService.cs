using Application.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant.Commands
{
    public interface IProductVariantCommandsService
    {
    }
    public class ProductVariantCommandsService : IProductVariantCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductVariantCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
    }
}
