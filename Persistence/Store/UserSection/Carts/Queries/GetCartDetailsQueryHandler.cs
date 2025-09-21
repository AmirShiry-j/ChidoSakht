using Application.Commons.Objects.Dtoes;
using Application.Store.UserSection.CartService.Queries;
using Domain.Carts;
using Domain.Products;
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
            var cartDetails = await _context.Carts.Where(p => p.UserId.Equals(request.UserId) && p.CartType.Equals(request.CartType))
    .Include(p => p.Items)
    .ThenInclude(p => p.ProductVariant)
    .ThenInclude(p => p.Product)
    .Select(p => new CartDetailsDto
    {
        CartId = p.Id,
        UserId = p.UserId,
        CartType = p.CartType,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        CartItems = p.Items.Select(s => new CartItemDto
        {
            CartItemId = s.Id,
            ProductId = s.ProductVariant.ProductId,
            ProductType = s.ProductVariant.ProductType,
            ProductVariantId = s.ProductVariantId,
            Quantity = s.Quantity,
            Price_LastKnown = s.Price_LastKnown,
            SpecialPrice_LastKnown = s.SpecialPrice_LastKnown,
            Stock = s.ProductVariant.Stock,
            ProductName = s.ProductVariant.Product.Name,
            NameIndexImage = s.ProductVariant.Product.NameIndexImage,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            Price_Now = s.ProductVariant.Price,
            SpecialPrice_Now = s.ProductVariant.SpecialPrice,

        }).ToList()
    })
    .FirstOrDefaultAsync();

            return cartDetails;
        }
    }
}
