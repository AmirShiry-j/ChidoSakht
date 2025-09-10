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
        Task<ResultDto<CartDetailsDto>> GetCartDetails(string UserId, CartStatus CartStatus);
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

        public async Task<ResultDto<CartDetailsDto>> GetCartDetails(string UserId, CartStatus CartStatus)
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
            var cartDetails = await _mediator.Send(new GetCartDetailsQuery(UserId, CartStatus));
            return new ResultDto<CartDetailsDto>
            {
                IsSuccess = true,
                Data = cartDetails
            };
        }
    }

    public class CartDetailsDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        //public string FullName { get; set; }
        public long TotalPrice { get; set; }
        public CartStatus CartStatus { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
    public class CartItemDto
    {
        public long Id { get; set; }
        public int? ProductVariantId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        //
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public int Stock { get; set; }
        //
    }
}
