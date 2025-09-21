using Application.Store.UserSection.CartService.Queries;
using Domain.Carts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Carts.Queries
{
    public class GetCartByCartIdQueryHandler : IRequestHandler<GetCartByCartIdQuery, Cart>
    {
        private readonly DataBaseContext _context;
        public GetCartByCartIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Cart> Handle(GetCartByCartIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.Where(p => p.Id.Equals(request.CartId))
                .FirstOrDefaultAsync(cancellationToken);

            return cart;
        }
    }
}
