using Domain.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.OrderService.Queries
{
    public class GetOrdersByFilterQuery : IRequest<List<OrderDto>>
    {
        public string? UserId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public GetOrdersByFilterQuery(string? UserId, OrderStatus? OrderStatus)
        {
            this.UserId = UserId;
            this.OrderStatus = OrderStatus;
        }
    }

    public class GetOrderDetailsByOrderIdQuery : IRequest<OrderDetailsDto>
    {
        public long OrderId { get; set; }
        public GetOrderDetailsByOrderIdQuery(long OrderId)
        {
            this.OrderId = OrderId;
        }
    }

    public class GetOrderByIdQuery : IRequest<Order>
    {
        public long OrderId { get; set; }
        public GetOrderByIdQuery(long OrderId)
        {
            this.OrderId = OrderId;
        }
    }
}
