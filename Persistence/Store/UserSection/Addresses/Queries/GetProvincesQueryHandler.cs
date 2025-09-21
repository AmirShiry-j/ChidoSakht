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
    public class GetProvincesQueryHandler : IRequestHandler<GetProvincesQuery, List<ProvinceDto>>
    {
        private readonly DataBaseContext _context;
        public GetProvincesQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProvinceDto>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Provinces.Select(p => new ProvinceDto
            {
                ProvinceId = p.ProvinceId,
                ProvinceName = p.ProvinceName
            }).ToListAsync();
        }
    }
}
