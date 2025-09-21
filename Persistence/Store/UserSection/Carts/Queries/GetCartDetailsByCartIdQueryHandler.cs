using Application.Store.UserSection.CartService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Carts.Queries
{
    public class GetCartDetailsByCartIdQueryHandler : IRequestHandler<GetCartDetailsByCartIdQuery, CartDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetCartDetailsByCartIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<CartDetailsDto> Handle(GetCartDetailsByCartIdQuery request, CancellationToken cancellationToken)
        {
            var cartDetails = await _context.Carts.Where(p => p.Id.Equals(request.CartId))
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
