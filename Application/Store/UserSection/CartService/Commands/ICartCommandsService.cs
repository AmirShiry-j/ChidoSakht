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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService.Commands
{
    public interface ICartCommandsService
    {
        Task<ResultDto> AddItemToCard(string UserId, int? ProductId, int? VariantId);
        Task<ResultDto> DeleteItemInCard(string UserId, long CartItemId);
        Task<ResultDto> UpdateQuantityAnItem(string UserId, UpdateQuantityOfItemInCartDto dto);
        Task<ResultDto> MoveAnItemInACartToAnotherCart(string UserId, MoveAnItemInCartToAnotherCartDto dto);
        Task<ResultDto> UpdatePricesInCartWithUserApproval(string UserId, long CartId);
        Task<ResultDto> UpdateNumberOfItemsWithUserApproval(string UserId, long CartId);
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
            var cartItem = await _mediator.Send(new GetCartItemWithCartQuery(CartItemId));

            //check exist
            if (cartItem is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check cart is for user
            if (!cartItem.Cart.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes این سبد خرید متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //check must'nt Closed
            if (!(cartItem.Cart.CartType.Equals(CartType.Open) || cartItem.Cart.CartType.Equals(CartType.Next)))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes سبد خرید بسته شده را نمیتوان تغییر داد",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Delete item
            await _mediator.Send(new DeleteCartItemCommand(cartItem));
            return new ResultDto
            {
                IsSuccess = true,

            };
        }

        public async Task<ResultDto> MoveAnItemInACartToAnotherCart(string UserId, MoveAnItemInCartToAnotherCartDto dto)
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
            var cartItem = await _mediator.Send(new GetCartItemWithCartQuery(dto.CartItemId));

            //check exist
            if (cartItem is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check cart is for user
            if (!cartItem.Cart.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes این سبد خرید متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //check must'nt Closed
            if (!(cartItem.Cart.CartType.Equals(CartType.Open) || cartItem.Cart.CartType.Equals(CartType.Next)))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes سبد خرید بسته شده را نمیتوان تغییر داد",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //check has before
            if (cartItem.Cart.CartType == dto.MoveToCartType)
            {
                return new ResultDto
                {
                    IsSuccess = true,
                };
            }

            //Move
            if (dto.MoveToCartType == CartType.Next)
            {
                var cart_Next = await _mediator.Send(new GetCartWithItemsQuery(UserId, CartType.Next));

                //create it if it is not exist
                if (cart_Next is null)
                {
                    //Define
                    cart_Next = new Domain.Carts.Cart
                    {
                        UserId = UserId,
                        CartType = CartType.Next,
                    };
                    //Create
                    var cartId = await _mediator.Send(new CreateACartWithItemCommand(cart_Next));

                    //change item
                    cartItem.CartId = cartId;

                    //Update item
                    await _mediator.Send(new UpdateCartItemCommand(cartItem));
                }
                else //add item to it
                {
                    //change item
                    cartItem.CartId = cart_Next.Id;
                    //if item was exist in cart before 
                    //var existing_item = cart_Next.Items.Where(p => p.ProductVariantId.Equals(cartItem.ProductVariantId)).FirstOrDefault();
                    //if (existing_item is not null)
                    //    cartItem.Quantity = existing_item.Quantity + cartItem.Quantity;
                    //else
                    //    cartItem.Quantity = cartItem.Quantity;

                    //updates
                    await _mediator.Send(new UpdateCartItemCommand(cartItem));
                }
            }
            else
            {
                var cart_Open = await _mediator.Send(new GetCartWithItemsQuery(UserId, CartType.Open));

                //create it if it is not exist
                if (cart_Open is null)
                {
                    //Define
                    cart_Open = new Domain.Carts.Cart
                    {
                        UserId = UserId,
                        CartType = CartType.Open,
                    };
                    //Create
                    var cartId = await _mediator.Send(new CreateACartWithItemCommand(cart_Open));

                    //change item
                    cartItem.CartId = cartId;

                    //Update item
                    await _mediator.Send(new UpdateCartItemCommand(cartItem));
                }
                else //add item to it
                {
                    //change item
                    cartItem.CartId = cart_Open.Id;
                    //if item was exist in cart before 
                    //var existing_item = cart_Open.Items.Where(p => p.ProductVariantId.Equals(cartItem.ProductVariantId)).FirstOrDefault();
                    //if (existing_item is not null)
                    //    cartItem.Quantity = existing_item.Quantity + cartItem.Quantity;
                    //else
                    //    cartItem.Quantity = cartItem.Quantity;

                    //updates
                    await _mediator.Send(new UpdateCartItemCommand(cartItem));
                }
            }

            //
            return new ResultDto
            {
                IsSuccess = true
            };
        }

        public async Task<ResultDto> UpdateQuantityAnItem(string UserId, UpdateQuantityOfItemInCartDto dto)
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
            var cartItem = await _mediator.Send(new GetCartItemWithCartQuery(dto.CartItemId));

            //check exist
            if (cartItem is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check cart is for user
            if (!cartItem.Cart.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes این سبد خرید متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //check must'nt Closed
            if (!(cartItem.Cart.CartType.Equals(CartType.Open) || cartItem.Cart.CartType.Equals(CartType.Next)))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes سبد خرید بسته شده را نمیتوان تغییر داد",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            ////Changes
            //Update Quentity
            if (dto.Behavior.HasValue)
            {
                if (dto.Behavior.Value == Behavior.Increase) //Increase
                {
                    if (cartItem.Quantity < int.MaxValue)
                        cartItem.Quantity++;
                }
                else //Decrease
                {
                    if (cartItem.Quantity > 0)
                        cartItem.Quantity--;
                }
            }
            else
            {//Set Quantity
                cartItem.Quantity = (int)dto.Quantity;
            }


            //Updates
            await _mediator.Send(new UpdateCartItemCommand(cartItem));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

        public async Task<ResultDto> UpdateNumberOfItemsWithUserApproval(string UserId, long CartId)
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

            //Get cart
            var cart = await _mediator.Send(new GetCartWithItemsAndVariantsByCartIdQuery(CartId));

            //check exist
            if (cart is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check cart is for user
            if (!cart.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes این سبد خرید متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            return null;
        }

        public async Task<ResultDto> UpdatePricesInCartWithUserApproval(string UserId, long CartId)
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

            //Get cart
            var cart = await _mediator.Send(new GetCartWithItemsAndVariantsByCartIdQuery(CartId));

            //check exist
            if (cart is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check cart is for user
            if (!cart.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes این سبد خرید متعلق به شما نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            return null;
        }
    }
}
public enum Behavior
{
    Increase = 1,
    Decrease = 2
}

public class UpdateQuantityOfItemInCartDto
{
    public long CartItemId { get; set; }
    public int? Quantity { get; set; }
    public Behavior? Behavior { get; set; }
}

public class MoveAnItemInCartToAnotherCartDto
{
    public long CartItemId { get; set; }
    public CartType MoveToCartType { get; set; }
}

