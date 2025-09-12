using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.AddressService.Commands
{
    public class CreateAddressCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public CreateAddressCommand(string Name, string userId)
        {
            this.Name = Name;
            UserId = userId;
        }
    }
    public class UpdateAddressCommand : IRequest
    {
        public Address Address { get; set; }
        public UpdateAddressCommand(Address Address)
        {
            this.Address = Address;
        }
    }
    public class DeleteAddressCommand : IRequest
    {
        public Address Address { get; set; }
        public DeleteAddressCommand(Address Address)
        {
            this.Address = Address;
        }
    }
}
