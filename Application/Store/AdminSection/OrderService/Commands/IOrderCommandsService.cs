using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.OrderService.Queries;
using Domain.Orders;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.OrderService.Commands
{
    public interface IOrderCommandsService
    {
        Task<ResultDto> UpdateStatusOfAnOrder(long OrderId, OrderStatus OrderStatus);
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

        public async Task<ResultDto> UpdateStatusOfAnOrder(long OrderId, OrderStatus OrderStatus)
        {
            //get order
            var order = await _mediator.Send(new GetOrderByIdQuery(OrderId));

            //check
            if (order is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //change
            order.OrderStatus = OrderStatus;
            //update
            await _mediator.Send(new UpdateOrderCommand(order));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

    }
}
