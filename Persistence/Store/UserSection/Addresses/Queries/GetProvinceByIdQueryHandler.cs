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
    public class GetProvinceByIdQueryHandler : IRequestHandler<GetProvinceByIdQuery, Province>
    {
        private readonly DataBaseContext _context;
        public GetProvinceByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Province> Handle(GetProvinceByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Provinces.FirstOrDefaultAsync(p => p.ProvinceId.Equals(request.ProvinceId));
        }
    }
}
