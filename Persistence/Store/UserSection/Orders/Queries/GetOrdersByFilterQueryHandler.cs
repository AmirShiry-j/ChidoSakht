using Application.Commons.Objects.Dtoes;
using Application.Store.UserSection.OrderService.Queries;
using Domain.Orders;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Orders.Queries
{
    public class GetOrdersByFilterQueryHandler : IRequestHandler<GetOrdersByFilterQuery, List<OrderDto>>
    {
        private readonly DataBaseContext _context;
        public GetOrdersByFilterQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<OrderDto>> Handle(GetOrdersByFilterQuery request, CancellationToken cancellationToken)
        {
            var prOrder = PredicateBuilder.True<Order>();

            //filter userid
            prOrder.And(p => p.UserId.Equals(request.UserId));

            //filter by status if it wasnot null
            if (request.OrderStatus is not null)
            {
                prOrder.And(p => p.OrderStatus.Equals(request.OrderStatus));
            }

            var orders = await _context.Orders.Where(prOrder).OrderByDescending(p => p.Id).Select(p => new OrderDto
            {
                CartId = p.CartId,
                CreatedAt = p.CreatedAt,
                DiscountAmout = p.DiscountAmout,
                FinalAmout = p.FinalAmout,
                OrderId = p.Id,
                OrderStatus = p.OrderStatus,
                SendBy = p.SendBy,
                SendCost = p.SendCost,
                TotalAmout = p.TotalAmout
            }).ToListAsync();

            return orders;
        }
    }
}
