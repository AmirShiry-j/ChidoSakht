using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.CartService.Commands;
using Application.Store.UserSection.CartService.Queries;
using Application.Store.UserSection.OrderService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.OrderService.Commands
{
    public interface IOrderCommandsService
    {

    }
    public class OrderCommandsService : IOrderCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public OrderCommandsService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }
    }
}
