using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
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

namespace Application.Store.UserSection.CartService.Queries
{
    public interface ICartQueriesService
    {
        Task<ResultDto<CartDetailsDto>> GetCartDetails(string UserId, CartType CartType);
    }
    public class CartQueriesService : ICartQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public CartQueriesService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        public async Task<ResultDto<CartDetailsDto>> GetCartDetails(string UserId, CartType CartType)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto<CartDetailsDto>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            ////Get Now cart
            //var cart = await _mediator.Send(new GetCartQuery(UserId, CartStatus));
            //if (cart is null)
            //{
            //    return new ResultDto<CartDetailsDto>
            //    {
            //        IsSuccess = true,
            //        Data = new CartDetailsDto { }
            //    };
            //}

            //Details
            var cartDetails = await _mediator.Send(new GetCartDetailsQuery(UserId, CartType));
            return new ResultDto<CartDetailsDto>
            {
                IsSuccess = true,
                Data = cartDetails
            };
        }
    }

    public class CartDetailsDto
    {
        public long CartId { get; set; }
        public string UserId { get; set; }
        public CartType CartType { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public long TotalPrice_LastKnown => CartItems.Sum(i => (i.LastKnownSpecialPrice is null ? i.LastKnownPrice : (long)i.LastKnownSpecialPrice) * i.Quantity);
        public long TotalPrice_Now => CartItems.Sum(i => (i.NowSpecialPrice is null ? i.NowPrice : (long)i.NowSpecialPrice) * i.Quantity);
        public bool AreAllThePricesUpToDate
        {
            get
            {
                return CartItems.All(p => p.AreThePricesUpToDate);
            }
        }
        //
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class CartItemDto
    {
        public long CartItemId { get; set; }
        public int ProductId { get; set; }
        public ProductType ProductType { get; set; }
        public int? ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string? NameIndexImage { get; set; }

        public int Quantity { get; set; }
        //
        public int Stock { get; set; }
        //
        public long LastKnownPrice { get; set; }
        public long? LastKnownSpecialPrice { get; set; }
        //
        public long NowPrice { get; set; }
        public long? NowSpecialPrice { get; set; }
        public bool AreThePricesUpToDate
        {
            get
            {
                return LastKnownPrice == NowPrice
    && Nullable.Equals(LastKnownSpecialPrice, NowSpecialPrice);
            }
        }
        //
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
