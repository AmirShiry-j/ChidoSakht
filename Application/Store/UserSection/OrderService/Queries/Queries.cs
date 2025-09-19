using Domain.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.OrderService.Queries
{
    public class GetOrdersByFilterQuery : IRequest<List<OrderDto>>
    {
        public string UserId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public GetOrdersByFilterQuery(string UserId, OrderStatus? OrderStatus)
        {
            this.UserId = UserId;
            this.OrderStatus = OrderStatus;
        }
    }
}
