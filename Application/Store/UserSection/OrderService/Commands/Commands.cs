using Application.Store.UserSection.CartService.Queries;
using Domain.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.OrderService.Commands
{
    public class CreateOrderByCartIdCommand : IRequest<long>
    {
        public Order Order { get; set; }
        public CreateOrderByCartIdCommand(Order Order)
        {
            this.Order = Order;
        }
    }
}
