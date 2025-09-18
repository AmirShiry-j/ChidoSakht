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
    public class GetCartWithItemsQuery : IRequest<Cart>
    {
        public string UserId { get; set; }
        public CartType CartType { get; set; }
        public GetCartWithItemsQuery(string UserId, CartType CartType)
        {
            this.UserId = UserId;
            this.CartType = CartType;
        }
    }

    public class GetCartQuery : IRequest<Cart>
    {
        public string UserId { get; set; }
        public CartType CartType { get; set; }
        public GetCartQuery(string UserId, CartType CartType)
        {
            this.UserId = UserId;
            this.CartType = CartType;
        }
    }

    public class GetCartDetailsQuery : IRequest<CartDetailsDto>
    {
        public string UserId { get; set; }
        public CartType CartType { get; set; }
        public GetCartDetailsQuery(string UserId, CartType CartType)
        {
            this.UserId = UserId;
            this.CartType = CartType;
        }
    }
    public class GetCartDetailsByCartIdQuery : IRequest<CartDetailsDto>
    {
        public long CartId { get; set; }
        public GetCartDetailsByCartIdQuery(long CartId)
        {
            this.CartId = CartId;
        }
    }

    public class GetCartItemByIdQuery : IRequest<CartItem>
    {
        public long Id { get; set; }
        public GetCartItemByIdQuery(long Id)
        {
            this.Id = Id;
        }
    }
    public class GetCartItemByIdAndUserIdInOpenAndNextCartQuery : IRequest<CartItem>
    {
        public long CartItemId { get; set; }
        public string UserId { get; set; }
        public GetCartItemByIdAndUserIdInOpenAndNextCartQuery(long CartItemId, string UserId)
        {
            this.CartItemId = CartItemId;
            this.UserId = UserId;
        }
    }
    public class GetCartItemWithCartQuery : IRequest<CartItem>
    {
        public long CartItemId { get; set; }
        public GetCartItemWithCartQuery(long CartItemId)
        {
            this.CartItemId = CartItemId;
        }
    }

    public class GetCartWithItemsAndVariantsByCartIdQuery : IRequest<Cart>
    {
        public long CartId { get; set; }
        public GetCartWithItemsAndVariantsByCartIdQuery(long CartId)
        {
            this.CartId = CartId;
        }
    }
}
