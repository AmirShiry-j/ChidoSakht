using Application.Store.UserSection.CartService.Queries;
using Domain.Carts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Carts.Queries
{
    public class GetCartItemByIdAndUserIdInOpenAndNextCartQueryHandler : IRequestHandler<GetCartItemByIdAndUserIdInOpenAndNextCartQuery, CartItem>
    {
        private readonly DataBaseContext _context;
        public GetCartItemByIdAndUserIdInOpenAndNextCartQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<CartItem> Handle(GetCartItemByIdAndUserIdInOpenAndNextCartQuery request, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems.Include(p => p.Cart)
                .FirstOrDefaultAsync(p => p.Id.Equals(request.CartItemId) && p.Cart.UserId.Equals(request.UserId) && (p.Cart.CartType.Equals(CartType.Open) || p.Cart.CartType.Equals(CartType.Next)));

            return cartItem;
        }
    }
}
