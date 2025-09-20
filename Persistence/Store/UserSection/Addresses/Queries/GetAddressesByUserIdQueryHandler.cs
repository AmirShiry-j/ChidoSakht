using Application.Store.UserSection.AddressService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Addresses.Queries
{
    public class GetAddressesByUserIdQueryHandler : IRequestHandler<GetAddressesByUserIdQuery, List<AddressDto>>
    {
        private readonly DataBaseContext _context;
        public GetAddressesByUserIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<AddressDto>> Handle(GetAddressesByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Addresses.Where(p => p.UserId.Equals(request.UserId)).Include(p => p.User)
                .Select(p => new AddressDto
                {
                    AddressId = p.Id,
                    Name = p.Name,
                    FullAddress = p.FullAddress,
                    PostalCode = p.PostalCode,
                    NameRecipient = p.WhoRecept.Equals(WhoRecept.Me) ? p.User.FullName : p.NameRecipient,
                    PhoneNumberRecipient = p.WhoRecept.Equals(WhoRecept.Me) ? p.User.PhoneNumber : p.PhoneNumberRecipient,
                }).ToListAsync();
        }
    }
}
