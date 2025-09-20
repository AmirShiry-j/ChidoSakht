using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.AddressService.Queries
{
    public interface IAddressQueriesService
    {
        Task<ResultDto<List<AddressDto>>> GetAddressesByUserId(string UserId);
        Task<ResultDto<AddressDetailsDto>> GetAddressByAddressId(string UserId, int AddressId);
    }
    public class AddressQueriesService : IAddressQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public AddressQueriesService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        public async Task<ResultDto<List<AddressDto>>> GetAddressesByUserId(string UserId)
        {
            //get data from db
            var addresses = await _mediator.Send(new GetAddressesByUserIdQuery(UserId));

            return new ResultDto<List<AddressDto>>
            {
                IsSuccess = true,
                Data = addresses
            };
        }

        public async Task<ResultDto<AddressDetailsDto>> GetAddressByAddressId(string UserId, int AddressId)
        {
            //get data from db
            var address = await _mediator.Send(new GetAddressDetailsByIdQuery(AddressId));

            //check exist
            if (address is null)
            {
                return new ResultDto<AddressDetailsDto>
                {
                    MessageEventType = Commons.Objects.MessageEventTypes.MessageEventType.NotFound
                };
            }

            //check address was for user
            if (!address.UserId.Equals(UserId))
            {
                return new ResultDto<AddressDetailsDto>
                {
                    Message = "Temp-Mes آدرس متعلق به این یوزر نیست",
                    MessageEventType = Commons.Objects.MessageEventTypes.MessageEventType.BadRequest
                };
            }

            return new ResultDto<AddressDetailsDto>
            {
                IsSuccess = true,
                Data = address
            };
        }
    }
    public class AddressDto
    {
        public int AddressId { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public string PostalCode { get; set; }
        public string NameRecipient { get; set; }
        public string PhoneNumberRecipient { get; set; }
    }
    public class AddressDetailsDto
    {
        public string UserId { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string FullAddress { get; set; }
        public string Pelak { get; set; }
        public string PostalCode { get; set; }
        public string? UnitNumber { get; set; }
        public WhoRecept WhoRecept { get; set; }
        public string NameRecipient { get; set; }
        public string PhoneNumberRecipient { get; set; }
    }
}
