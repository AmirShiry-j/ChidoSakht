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
    public class GetCartWithItemsAndVariantsByCartIdQueryHandler : IRequestHandler<GetCartWithItemsAndVariantsByCartIdQuery, Cart>
    {
        private readonly DataBaseContext _context;
        public GetCartWithItemsAndVariantsByCartIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Cart> Handle(GetCartWithItemsAndVariantsByCartIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.Include(p => p.Items).ThenInclude(p => p.ProductVariant)
                .FirstOrDefaultAsync(p => p.Id.Equals(request.CartId));

            return cart;
        }
    }
}
