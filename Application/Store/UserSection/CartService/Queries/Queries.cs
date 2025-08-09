using Application.Store.UserSection.CartService.Commands;
using AutoMapper;
using Domain.Carts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService.Queries
{
    public class GetCartQuery : IRequest<CartDto>
    {
        public string UserId { get; set; }
    }

    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserIdAsync(request.UserId);
            return cart != null ? _mapper.Map<CartDto>(cart) : null;
        }
    }

    public interface ICartRepository
    {
        Task<Cart> GetByUserIdAsync(string userId);
        Task SaveAsync(Cart cart);
        // متدهای دیگر مثل Delete
    }
}
