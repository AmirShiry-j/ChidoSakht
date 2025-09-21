using Application.Store.UserSection.CartService.Queries;
using Domain.Carts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Carts.Queries
{
    public class GetCartItemByIdQueryHandler : IRequestHandler<GetCartItemByIdQuery, CartItem>
    {
        private readonly DataBaseContext _context;
        public GetCartItemByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<CartItem> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            return cartItem;
        }
    }
}
