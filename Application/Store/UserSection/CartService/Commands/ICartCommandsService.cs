using Application.Commons.Interfaces.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CartService.Commands
{
    public interface ICartCommandsService
    {
    }
    public class CartCommandsService : ICartCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public CartCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
    }

    public class CartDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class CartItemDto
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
