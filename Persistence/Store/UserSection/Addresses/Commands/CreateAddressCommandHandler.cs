using Application.Store.UserSection.AddressService.Commands;
using Domain.Users;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Addresses.Commands
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, int>
    {
        private readonly DataBaseContext _context;
        public CreateAddressCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            //Define
            var newAddress = new Address
            {
                Name = request.Name,
                UserId = request.UserId,
            };

            //Add and save in DB
            await _context.Addresses.AddAsync(newAddress);
            await _context.SaveChangesAsync(cancellationToken);

            return newAddress.Id;
        }
    }
}
