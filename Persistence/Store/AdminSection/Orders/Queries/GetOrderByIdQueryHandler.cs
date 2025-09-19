using Application.Store.AdminSection.OrderService.Queries;
using Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Orders.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly DataBaseContext _context;
        public GetOrderByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders.FirstOrDefaultAsync(p => p.Id.Equals(request.OrderId));
        }
    }
}
