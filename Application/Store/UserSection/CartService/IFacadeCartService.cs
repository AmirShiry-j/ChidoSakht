using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.CartService.Commands;
using Application.Store.UserSection.CartService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService
{
    public interface IFacadeCartService
    {
        ICartCommandsService CartCommandsService { get; }
        ICartQueriesService CartQueriesService { get; }
    }
    public class FacadeCartService : IFacadeCartService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public FacadeCartService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        //Commands
        private ICartCommandsService _CartCommandsService;
        public ICartCommandsService CartCommandsService
        {
            get
            {
                return _CartCommandsService = _CartCommandsService ?? new CartCommandsService(_mediator, _localizationService, _userManager);
            }
        }

        //Queries
        private ICartQueriesService _CartQueriesService;
        public ICartQueriesService CartQueriesService
        {
            get
            {
                return _CartQueriesService = _CartQueriesService ?? new CartQueriesService(_mediator, _localizationService, _userManager);
            }
        }
    }
}
