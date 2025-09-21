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
        public Address Address { get; set; }
        public CreateAddressCommand(Address Address)
        {
            this.Address = Address;
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
