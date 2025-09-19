using Application.Store.AdminSection.OrderService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Orders.Queries
{
    public class GetOrderDetailsByOrderIdQueryHandler : IRequestHandler<GetOrderDetailsByOrderIdQuery, OrderDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetOrderDetailsByOrderIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<OrderDetailsDto> Handle(GetOrderDetailsByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var orderDetails = await _context.Orders.Where(p => p.Id.Equals(request.OrderId))
               .Include(p => p.Address)
               .Include(p => p.User)
               .Include(p => p.Payment)
               .Include(p => p.Items)
               .ThenInclude(p => p.ProductVariant)
               .Select(p => new OrderDetailsDto
               {
                   OrderId = p.Id,
                   UserId = p.UserId,
                   FullName = p.User.FullName,
                   PhoneNumber = p.User.PhoneNumber,
                   CartId = p.CartId,
                   CreatedAt = p.CreatedAt,
                   DiscountAmout = p.DiscountAmout,
                   FinalAmout = p.FinalAmout,
                   SendBy = p.SendBy,
                   SendCost = p.SendCost,
                   TotalAmout = p.TotalAmout,
                   Address = new AddressDetailsDto
                   {
                       AddressId = p.AddressId,
                       Name = p.Address.Name,
                   },
                   OrderStatus = p.OrderStatus,
                   Payment = p.Payment == null ? null : new PaymentDto
                   {
                       PaymentId = p.Payment.Id,
                   },
                   OrderItems = p.Items.Select(s => new OrderItemDto
                   {
                       OrderItemId = s.Id,
                       Price = s.Price,
                       ProductVariantId = s.ProductVariantId,
                       Quantity = s.Quantity,
                       SpecialPrice = s.SpecialPrice
                   }).ToList()
               }).FirstOrDefaultAsync(cancellationToken);

            return orderDetails;
        }
    }
}
