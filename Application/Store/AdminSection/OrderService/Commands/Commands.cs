using Domain.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.OrderService.Commands
{
    public class UpdateOrderCommand : IRequest
    {
        public Order Order { get; set; }
        public UpdateOrderCommand(Order Order)
        {
            this.Order = Order;
        }
    }
}
