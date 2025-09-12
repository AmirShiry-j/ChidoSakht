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
    public class CreateACartWithItemCommand : IRequest<long>
    {
        public Cart Cart { get; set; }
        public CreateACartWithItemCommand(Cart Cart)
        {
            this.Cart = Cart;
        }
    }
    public class UpdateACartWithItemCommand : IRequest
    {
        public Cart Cart { get; set; }
        public UpdateACartWithItemCommand(Cart Cart)
        {
            this.Cart = Cart;
        }
    }

    public class DeleteCartItemCommand : IRequest
    {
        public CartItem CartItem { get; set; }
        public DeleteCartItemCommand(CartItem CartItem)
        {
            this.CartItem = CartItem;
        }
    }
}
