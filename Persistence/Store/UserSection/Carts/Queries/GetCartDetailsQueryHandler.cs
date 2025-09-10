using Application.Store.UserSection.CartService.Queries;
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
    public class GetCartDetailsQueryHandler : IRequestHandler<GetCartDetailsQuery, CartDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetCartDetailsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<CartDetailsDto> Handle(GetCartDetailsQuery request, CancellationToken cancellationToken)
        {
            var cartDetails = await _context.Carts.Where(p => p.UserId.Equals(request.UserId) && p.CartStatus.Equals(request.CartStatus))
    .Include(p => p.Items)
    .Select(p=> new CartDetailsDto
    {
        Id=p.Id,
        UserId=p.UserId,
        CartStatus=p.CartStatus,
        TotalPrice=p.TotalPrice,
        CartItems = p.Items.Select(s=>new CartItemDto
        {
            Id=s.Id,
            ProductId=s.ProductVariant.ProductId,
            ProductType = s.ProductVariant.ProductType,
            ProductVariantId= s.ProductVariantId,
            Quantity=s.Quantity,
            SpecialPrice = s.ProductVariant.SpecialPrice,
            Stock= s.ProductVariant.Stock,            
        }).ToList()
    })
    .FirstOrDefaultAsync();

            return cartDetails;
        }
    }
}
