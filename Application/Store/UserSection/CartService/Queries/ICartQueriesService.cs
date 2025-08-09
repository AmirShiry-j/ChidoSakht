using Application.Commons.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService.Queries
{
    public interface ICartQueriesService
    {
    }
    public class CartQueriesService : ICartQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public CartQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
    }
}
