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
}
