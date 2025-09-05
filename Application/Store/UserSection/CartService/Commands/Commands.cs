using Application.Store.UserSection.CartService.Queries;
using AutoMapper;
using Domain.Carts;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService.Commands
{
    public class AddToCartCommand : IRequest<CartDto>
    {
        public string UserId { get; set; }
        public int PoductVariantId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AddToCartCommandHandler(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            //var product = await _productRepository.GetByIdAsync(Guid.NewGuid());
            //if (product == null) throw new Exception("Product not found");

            //var cart = await _cartRepository.GetByUserIdAsync(request.UserId) ?? new Cart(request.UserId);
            //cart.AddItem(product, request.Quantity);

            //await _cartRepository.SaveAsync(cart);
            //return _mapper.Map<CartDto>(cart);
            return null;
        }
    }

    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
    }
}
