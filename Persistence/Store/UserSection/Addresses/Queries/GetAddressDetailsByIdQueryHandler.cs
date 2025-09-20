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
    public class GetAddressDetailsByIdQueryHandler : IRequestHandler<GetAddressDetailsByIdQuery, AddressDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetAddressDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<AddressDetailsDto> Handle(GetAddressDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Addresses.Where(p => p.Id.Equals(request.Id))
                .Include(p => p.User)
                .Include(p => p.City)
                .ThenInclude(p => p.Province)
                .Select(p => new AddressDetailsDto
                {
                    UserId = p.UserId,
                    AddressId = p.Id,
                    Name = p.Name,
                    FullAddress = p.FullAddress,
                    PostalCode = p.PostalCode,
                    NameRecipient = p.WhoRecept.Equals(WhoRecept.Me) ? p.User.FullName : p.NameRecipient,
                    PhoneNumberRecipient = p.WhoRecept.Equals(WhoRecept.Me) ? p.User.PhoneNumber : p.PhoneNumberRecipient,
                    CityId = p.CityId,
                    CityName = p.City.CityName,
                    ProvinceId = p.City.ProvinceId,
                    ProvinceName = p.City.Province.ProvinceName,
                    Pelak = p.Pelak,
                    UnitNumber = p.UnitNumber,
                    WhoRecept = p.WhoRecept
                }).FirstOrDefaultAsync();
        }
    }
}
