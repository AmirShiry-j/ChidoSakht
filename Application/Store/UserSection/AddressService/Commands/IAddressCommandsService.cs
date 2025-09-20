using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductAttribute.Commands;
using Application.Store.UserSection.AddressService.Queries;
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
using System.Xml.Linq;

namespace Application.Store.UserSection.AddressService.Commands
{
    public interface IAddressCommandsService
    {
        Task<ResultDto<int>> Create(string UserId, CreateAddressDto dto);
        Task<ResultDto> Update(string UserId, UpdateAddressDto dto);
        Task<ResultDto> Delete(string UserId, int AddressId);
    }
    public class AddressCommandsService : IAddressCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public AddressCommandsService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        public async Task<ResultDto<int>> Create(string UserId, CreateAddressDto dto)
        {
            //get User
            var user = _userManager.Users.Where(p => p.Id.Equals(UserId)).FirstOrDefault();
            if (user is null)
            {
                return new ResultDto<int>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Define
            var address = new Address()
            {
                Name = dto.Name,
                CityId = dto.CityId,
                FullAddress = dto.FullAddress,
                Pelak = dto.Pelak,
                PostalCode = dto.PostalCode,
                UnitNumber = dto.UnitNumber,
                WhoRecept = dto.WhoRecept,
                UserId = UserId
            };
            if (dto.WhoRecept == WhoRecept.Me)
            {
                address.WhoRecept = WhoRecept.Me;
                address.NameRecipient = null;
                address.PhoneNumberRecipient = null;
            }
            else
            {
                address.WhoRecept = WhoRecept.Other;
                address.NameRecipient = dto.NameRecipient;
                address.PhoneNumberRecipient = dto.PhoneNumberRecipient;
            }
            //Create
            var addressId = await _mediator.Send(new CreateAddressCommand(address));

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = addressId,
                MessageEventType = MessageEventType.Created
            };
        }

        public async Task<ResultDto> Delete(string UserId, int AddressId)
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

            //get from db
            var address = await _mediator.Send(new GetAddressByIdQuery(AddressId));

            //check exist
            if (address is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check address was for user
            if (!address.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "temp-Mes آدرس متعلق به این یوزر نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Check is using address?

            //Delete
            await _mediator.Send(new DeleteAddressCommand(address));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

        public async Task<ResultDto> Update(string UserId, UpdateAddressDto dto)
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

            //get from db
            var address = await _mediator.Send(new GetAddressByIdQuery(dto.AddressId));

            //check exist
            if (address is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //check address was for user
            if (!address.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "temp-Mes آدرس متعلق به این یوزر نیست",
                    MessageEventType = MessageEventType.BadRequest
                };
            }

            //Check is using address?

            //change
            address.Name = dto.Name;
            address.CityId = dto.CityId;
            address.FullAddress = dto.FullAddress;
            address.PostalCode = dto.PostalCode;
            address.Pelak = dto.Pelak;
            address.UnitNumber = dto.UnitNumber;
            if (dto.WhoRecept == WhoRecept.Me)
            {
                address.WhoRecept = WhoRecept.Me;
                address.NameRecipient = null;
                address.PhoneNumberRecipient = null;
            }
            else
            {
                address.WhoRecept = WhoRecept.Other;
                address.NameRecipient = dto.NameRecipient;
                address.PhoneNumberRecipient = dto.PhoneNumberRecipient;
            }

            //Update
            await _mediator.Send(new UpdateAddressCommand(address));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }
    }

    public class CreateAddressDto
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public string FullAddress { get; set; }
        public string Pelak { get; set; }
        public string PostalCode { get; set; }
        public string? UnitNumber { get; set; }
        public WhoRecept WhoRecept { get; set; }
        public string? NameRecipient { get; set; }
        public string? PhoneNumberRecipient { get; set; }
    }
    public class UpdateAddressDto
    {
        public int AddressId { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string FullAddress { get; set; }
        public string Pelak { get; set; }
        public string PostalCode { get; set; }
        public string? UnitNumber { get; set; }
        public WhoRecept WhoRecept { get; set; }
        public string? NameRecipient { get; set; }
        public string? PhoneNumberRecipient { get; set; }
    }
}
