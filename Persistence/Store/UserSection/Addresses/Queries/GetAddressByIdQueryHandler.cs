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
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Address>
    {
        private readonly DataBaseContext _context;
        public GetAddressByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Address> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Addresses.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));
        }
    }
}
