using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.OrderService.Commands;
using Application.Store.AdminSection.OrderService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.OrderService
{
    public interface IFacadeAdminOrderService
    {
        IOrderCommandsService OrderCommandsService { get; }
        IOrderQueriesService OrderQueriesService { get; }
    }
    public class FacadeAdminOrderService : IFacadeAdminOrderService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public FacadeAdminOrderService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }


        //Commands
        private IOrderCommandsService _OrderCommandsService;
        public IOrderCommandsService OrderCommandsService
        {
            get
            {
                return _OrderCommandsService = _OrderCommandsService ?? new OrderCommandsService(_mediator, _localizationService, _userManager);
            }
        }

        //Queries
        private IOrderQueriesService _OrderQueriesService;
        public IOrderQueriesService OrderQueriesService
        {
            get
            {
                return _OrderQueriesService = _OrderQueriesService ?? new OrderQueriesService(_mediator, _localizationService, _userManager);
            }
        }
    }
}
