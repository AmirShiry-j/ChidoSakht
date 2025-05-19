using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.RoleService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.ProductService.Commands
{
    public interface IProductCommandsService
    {
        Task<ResultDto<int>> Upsert(int? Id, string Name);
    }
    public class ProductCommandsService : IProductCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ProductCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<int>> Upsert(int? Id, string Name)
        {
            //Insert product
            var productId = await _mediator.Send(new InsertProductCommand(Name));
            if (productId is null)
            {
                return new ResultDto<int>
                {
                    Message = _localizationService.GetMessageProduct(MessageKeysProduct.CreatedWasUnSuccess.ToString()),
                };
            }

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = (int)productId
            };
        }
    }
}
