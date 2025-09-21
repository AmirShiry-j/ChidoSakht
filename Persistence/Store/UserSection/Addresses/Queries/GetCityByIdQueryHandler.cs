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
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, City>
    {
        private readonly DataBaseContext _context;
        public GetCityByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<City> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cities.FirstOrDefaultAsync(p => p.CityId.Equals(request.CityId), cancellationToken);
        }
    }
}
