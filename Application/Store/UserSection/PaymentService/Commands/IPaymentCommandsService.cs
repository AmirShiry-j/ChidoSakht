using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.OrderService.Commands;
using Application.Store.UserSection.OrderService.Queries;
using Domain.Orders;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.PaymentService.Commands
{
    public interface IPaymentCommandsService
    {
        Task<ResultDto<long>> PayingForAnOrder(string UserId, long OrderId);
    }
    public class PaymentCommandsService : IPaymentCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public PaymentCommandsService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        public async Task<ResultDto<long>> PayingForAnOrder(string UserId, long OrderId)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto<long>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get order
            var order = await _mediator.Send(new GetOrderByIdQuery(OrderId));
            if (order is null)
            {
                return new ResultDto<long>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //checks
            if (!order.OrderStatus.Equals(OrderStatus.PendingPayment))
            {
                return new ResultDto<long>
                {
                    Message = "Temp-Mes این سفارش در وضعیت مناسب برای پرداخت نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Define payment
            var payment = new Payment
            {
                Amout = order.TotalAmout,
                OrderId = OrderId,
                PaymentMethod = PaymentMethod.BankPort,
                PaymentStatus = PaymentStatus.Success,
                TransactionId = new Random().Next(),
            };
            //Create
            var paymentId = await _mediator.Send(new CreatePaymentCommand(payment));

            //change
            order.OrderStatus = OrderStatus.Paid;
            //update
            await _mediator.Send(new UpdateOrderCommand(order));

            return new ResultDto<long>
            {
                IsSuccess = true,
                Data = paymentId
            };
        }
    }
}
