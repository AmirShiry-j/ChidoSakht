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
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Cart>
    {
        private readonly DataBaseContext _context;
        public GetCartQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Cart> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.Where(p => p.UserId.Equals(request.UserId) && p.CartStatus.Equals(request.CartStatus))
                .FirstOrDefaultAsync();

            return cart;
        }
    }
}
