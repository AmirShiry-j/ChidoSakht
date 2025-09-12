using Application.Store.UserSection.AddressService.Queries;
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
    public class GetAddressDetailsByIdQueryHandler : IRequestHandler<GetAddressDetailsByIdQuery, AddressDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetAddressDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<AddressDetailsDto> Handle(GetAddressDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Addresses.Where(p => p.Id.Equals(request.Id)).Select(p => new AddressDetailsDto
            {
                AddressId = p.Id,
                Name = p.Name,
                UserId = p.UserId,
            }).FirstOrDefaultAsync();
        }
    }
}
