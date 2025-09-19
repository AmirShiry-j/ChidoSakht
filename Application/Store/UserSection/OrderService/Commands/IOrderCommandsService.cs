using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.AddressService.Queries;
using Application.Store.UserSection.CartService.Commands;
using Application.Store.UserSection.CartService.Queries;
using Application.Store.UserSection.OrderService.Queries;
using Domain.Carts;
using Domain.Orders;
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
        Task<ResultDto<long>> CreateOrder(string UserId, CreateOrderDto dto);
        Task<ResultDto> CancelOrder(string UserId, long OrderId);
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

        public async Task<ResultDto> CancelOrder(string UserId, long OrderId)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

        }

        public async Task<ResultDto<long>> CreateOrder(string UserId, CreateOrderDto dto)
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

            //get cart by details
            var cartByDetails = await _mediator.Send(new GetCartDetailsByCartIdQuery(dto.CartId));
            if (cartByDetails is null)
            {
                return new ResultDto<long>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            ////Checks
            if (!cartByDetails.UserId.Equals(UserId))
            {
                return new ResultDto<long>
                {
                    Message = "temp-mes این سبد خرید متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }
            if (!cartByDetails.CartType.Equals(CartType.Open))
            {
                if (cartByDetails.CartType.Equals(CartType.Ordered))
                {
                    return new ResultDto<long>
                    {
                        Message = "temp-mes این سبد خرید قبلا تبدیل به یک سفارش شده",
                        MessageEventType = MessageEventType.BadRequest
                    };
                }
                return new ResultDto<long>
                {
                    Message = "temp-mes این سبد خرید جزوه سبد خرید های جاری نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }
            if (!cartByDetails.AreAllThePricesUpToDate)
            {
                return new ResultDto<long>
                {
                    Message = "temp-mes قیمت محصولات موجود در سبد خرید تغییر کرده اند و بروز نیستند. لطفا سبد خرید رو با زدن دکمه بازسازی کنید",
                    MessageEventType = MessageEventType.BadRequest
                };
            }
            if (!cartByDetails.DoWeHaveEnoughInventoryForEverything)
            {
                return new ResultDto<long>
                {
                    Message = "temp-mes بعضی کالا ها به تعداد که در حال حاظر نیاز دارید موجود نیستند. لطفا سبد خرید رو با زدن دکمه بازسازی کنید",
                    MessageEventType = MessageEventType.BadRequest
                };
            }
            var address = await _mediator.Send(new GetAddressByIdQuery(dto.AddressId));
            if (!address.UserId.Equals(UserId))
            {
                return new ResultDto<long>
                {
                    Message = "temp-mes این آدرس متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //define order
            var order = new Order()
            {
                AddressId = address.Id,
                CartId = dto.CartId,
                SendBy = dto.SendBy,
                TotalAmout = cartByDetails.TotalAmount_Now,
                DiscountAmout = cartByDetails.TotalDiscountAmount_Now,
                FinalAmout = cartByDetails.FinalTotalAmout_Now,
                OrderStatus = OrderStatus.PendingPayment,
                SendCost = dto.SendBy == SendBy.Tipax ? 50000 : 34000,
                UserId = UserId,
                Items = cartByDetails.CartItems.Select(p => new OrderItem
                {
                    ProductVariantId = p.ProductVariantId,
                    Price = p.Price_Now,
                    Quantity = p.Quantity,
                    SpecialPrice = p.SpecialPrice_Now
                }).ToList()
            };
            //create order
            var orderId = await _mediator.Send(new CreateOrderByCartIdCommand(order));
            //change type of cart
            var cart = await _mediator.Send(new GetCartByCartIdQuery(dto.CartId));
            cart.CartType = CartType.Ordered;
            await _mediator.Send(new UpdateACartWithItemCommand(cart));

            return new ResultDto<long>
            {
                IsSuccess = true,
                Data = orderId,
                MessageEventType = MessageEventType.Created
            };
        }

    }
    public class CreateOrderDto
    {
        public long CartId { get; set; }
        public int AddressId { get; set; }
        public SendBy SendBy { get; set; }
    }
}
