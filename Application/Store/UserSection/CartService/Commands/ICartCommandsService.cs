using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.CategoryService.Queries;
using Application.Store.AdminSection.CommentService.Commands;
using Application.Store.AdminSection.ProductVariant.Queries;
using Application.Store.UserSection.CartService.Queries;
using Application.Store.UserSection.CommentService.Queries;
using Application.Store.UserSection.ProductService.Queries;
using Application.Store.UserSection.ProductVariantService.Queries;
using Domain.Carts;
using Domain.Products;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService.Commands
{
    public interface ICartCommandsService
    {
        Task<ResultDto> AddItemToCard(string UserId, int? ProductId, int? VariantId);
        Task<ResultDto> DeleteItemInCard(string UserId, long CartItemId);
    }
    public class CartCommandsService : ICartCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public CartCommandsService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }


        public async Task<ResultDto> AddItemToCard(string UserId, int? ProductId, int? VariantId)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            var variant = new ProductVariant();

            //get Entity
            if (VariantId is not null)
            {
                variant = await _mediator.Send(new GetProductVariantByInUserSectionIdQuery((int)VariantId));
                if (variant is null)
                {
                    return new ResultDto
                    {
                        MessageEventType = MessageEventType.NotFound
                    };
                }

                if (!variant.ProductType.Equals(ProductType.Variable))
                {
                    return new ResultDto
                    {
                        Message = "Temp-Mes این واریانت آیدی متعلق به یک محصول متغیر نیست",
                        MessageEventType = MessageEventType.BadRequest
                    };
                }

                if (variant.Stock <= 0)
                {
                    return new ResultDto
                    {
                        Message = "Temp-Mes کالا مورد نظر فعلا در انبار موجود نیست. به همین علت نمیتوان آن را به سبد خرید اضافه کرد",
                        MessageEventType = MessageEventType.BadRequest
                    };
                }
            }
            else if (ProductId is not null)
            {
                var product = await _mediator.Send(new GetProductByIdInUserSectionQuery((int)ProductId));
                if (product is null)
                {
                    return new ResultDto
                    {
                        MessageEventType = MessageEventType.NotFound
                    };
                }

                if (!product.ProductType.Equals(ProductType.Sample))
                {
                    return new ResultDto
                    {
                        Message = "Temp-Mes این آیدی یک محصول ساده نیست",
                        MessageEventType = MessageEventType.BadRequest
                    };
                }

                variant = await _mediator.Send(new GetProductVariantTypeSampleByProductIdInUserSectionQuery((int)ProductId));
                if (variant is null)
                {
                    return new ResultDto
                    {
                        MessageEventType = MessageEventType.NotFound
                    };
                }

                if (variant.Stock <= 0)
                {
                    return new ResultDto
                    {
                        Message = "Temp-Mes کالا مورد نظر فعلا در انبار موجود نیست. به همین علت نمیتوان آن را به سبد خرید اضافه کرد"
                    };
                }
            }
            else
            {
                return new ResultDto
                {
                    Message = "Temp-Mes اطلاعات نادرست است"
                };
            }

            //Get Now cart
            var cart = await _mediator.Send(new GetCartWithItemsQuery(UserId, CartType.Open));

            //create it if it is not exist
            if (cart is null)
            {
                cart = new Domain.Carts.Cart
                {
                    UserId = UserId,
                    CartType = CartType.Open,
                };
                cart.AddItem(variant, 1);
                //
                var cartId = await _mediator.Send(new CreateACartWithItemCommand(cart));
            }
            else //add item to it
            {
                cart.AddItem(variant, 1);
                //
                await _mediator.Send(new UpdateACartWithItemCommand(cart));
            }

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Created
            };
        }
        public async Task<ResultDto> DeleteItemInCard(string UserId, long CartItemId)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }


            //Get cartItem
            var cartItem = await _mediator.Send(new GetCartItemByIdAndUserIdInOpenAndNextCartQuery(CartItemId, UserId));

            //create it if it is not exist
            if (cartItem is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            await _mediator.Send(new DeleteCartItemCommand(cartItem));
            return new ResultDto
            {
                IsSuccess = true,

            };
        }
    }
}
public enum Behavior
{
    Increase = 1,
    Decrease = 2
}

//public class CartDto
//{
//    public long Id { get; set; }
//    public string UserId { get; set; }
//    public List<CartItemDto> Items { get; set; }
//    public decimal TotalPrice { get; set; }
//}
//public class CartItemDto
//{
//    public int VariantId { get; set; }
//    public int ProductId { get; set; }
//    public string ProductName { get; set; }
//    public long Price { get; set; }
//    public int Quantity { get; set; }
//}

