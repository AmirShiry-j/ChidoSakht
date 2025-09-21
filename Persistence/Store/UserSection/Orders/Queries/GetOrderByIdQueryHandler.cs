using Application.Store.UserSection.OrderService.Queries;
using Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Orders.Queries
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
