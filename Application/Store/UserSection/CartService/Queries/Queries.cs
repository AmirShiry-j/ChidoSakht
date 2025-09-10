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
        public CartStatus CartStatus { get; set; }
        public GetCartWithItemsQuery(string UserId, CartStatus CartStatus)
        {
            this.UserId = UserId;
            this.CartStatus = CartStatus;
        }
    }

    public class GetCartQuery : IRequest<Cart>
    {
        public string UserId { get; set; }
        public CartStatus CartStatus { get; set; }
        public GetCartQuery(string UserId, CartStatus CartStatus)
        {
            this.UserId = UserId;
            this.CartStatus = CartStatus;
        }
    }

    public class GetCartDetailsQuery : IRequest<CartDetailsDto>
    {
        public string UserId { get; set; }
        public CartStatus CartStatus { get; set; }
        public GetCartDetailsQuery(string UserId, CartStatus CartStatus)
        {
            this.UserId = UserId;
            this.CartStatus = CartStatus;
        }
    }
}
