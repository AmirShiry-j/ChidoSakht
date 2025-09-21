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
    public class GetCartWithItemsQueryHandler : IRequestHandler<GetCartWithItemsQuery, Cart>
    {
        private readonly DataBaseContext _context;
        public GetCartWithItemsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Cart> Handle(GetCartWithItemsQuery request, CancellationToken cancellationToken)
        {
            var cartWithItems = await _context.Carts.Where(p => p.UserId.Equals(request.UserId) && p.CartType.Equals(request.CartType))
                .Include(p => p.Items)
                .FirstOrDefaultAsync();

            return cartWithItems;
        }
    }
}
