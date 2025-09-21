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
    public class GetCitiesByProvinceIdQueryHandler : IRequestHandler<GetCitiesByProvinceIdQuery, List<CityDto>>
    {
        private readonly DataBaseContext _context;
        public GetCitiesByProvinceIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<CityDto>> Handle(GetCitiesByProvinceIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cities.Where(p => p.ProvinceId.Equals(request.ProvinceId)).Select(p => new CityDto
            {
                CityId = p.CityId,
                CityName = p.CityName
            })
                .OrderBy(p => p.CityName)
                .ToListAsync();
        }
    }
}
