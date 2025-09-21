using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CartService.Queries;
using Domain.Carts;
using Domain.Orders;
using Domain.Products;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.OrderService.Queries
{
    public interface IOrderQueriesService
    {
        Task<ResultDto<List<OrderDto>>> GetOrders(string UserId, OrderStatus? OrderStatus);
        Task<ResultDto<OrderDetailsDto>> GetOrderDetailsById(string UserId, long OrderId);
    }
    public class OrderQueriesService : IOrderQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public OrderQueriesService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }
        public async Task<ResultDto<List<OrderDto>>> GetOrders(string UserId, OrderStatus? OrderStatus)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto<List<OrderDto>>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Orders
            var orders = await _mediator.Send(new GetOrdersByFilterQuery(UserId, OrderStatus));
            return new ResultDto<List<OrderDto>>
            {
                IsSuccess = true,
                Data = orders
            };
        }

        public async Task<ResultDto<OrderDetailsDto>> GetOrderDetailsById(string UserId, long OrderId)
        {
            //get User
            var order = await _mediator.Send(new GetOrderDetailsByOrderIdAndUserIdQuery(OrderId, UserId));

            if (order is null)
            {
                return new ResultDto<OrderDetailsDto>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            return new ResultDto<OrderDetailsDto>
            {
                IsSuccess = true,
                Data = order
            };

        }
    }

    public class OrderDto
    {
        public long OrderId { get; set; }
        public long CartId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public long TotalAmout { get; set; }
        public long DiscountAmout { get; set; }
        public SendBy SendBy { get; set; }
        public long SendCost { get; set; }
        public long FinalAmout { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class OrderDetailsDto
    {
        public long OrderId { get; set; }
        public long CartId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public long TotalAmout { get; set; }
        public long DiscountAmout { get; set; }
        public SendBy SendBy { get; set; }
        public long SendCost { get; set; }
        public long FinalAmout { get; set; }
        public DateTime CreatedAt { get; set; }
        public AddressDetailsDto Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public PaymentDto Payment { get; set; }
    }
    public class AddressDetailsDto
    {
        public int AddressId { get; set; }
        public string Name { get; set; }
    }
    public class OrderItemDto
    {
        public long OrderItemId { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int ProductVariantId { get; set; }

    }
    public class PaymentDto
    {
        public long PaymentId { get; set; }
    }
}
