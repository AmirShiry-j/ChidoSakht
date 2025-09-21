using Application.Store.UserSection.CartService.Queries;
using Domain.Carts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Carts.Queries
{
    public class GetCartItemWithCartQueryHandler : IRequestHandler<GetCartItemWithCartQuery, CartItem>
    {
        private readonly DataBaseContext _context;
        public GetCartItemWithCartQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<CartItem> Handle(GetCartItemWithCartQuery request, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems.Include(p => p.Cart)
    .FirstOrDefaultAsync(p => p.Id.Equals(request.CartItemId));

            return cartItem;
        }
    }
}
