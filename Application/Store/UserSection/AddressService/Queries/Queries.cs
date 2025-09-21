using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.AddressService.Queries
{
    public class GetAddressesByUserIdQuery : IRequest<List<AddressDto>>
    {
        public string UserId { get; set; }
        public GetAddressesByUserIdQuery(string UserId)
        {
            this.UserId = UserId;
        }
    }

    public class GetAddressByIdQuery : IRequest<Address>
    {
        public int Id { get; set; }
        public GetAddressByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }

    public class GetAddressDetailsByIdQuery : IRequest<AddressDetailsDto>
    {
        public int Id { get; set; }
        public GetAddressDetailsByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }

    public class GetProvincesQuery : IRequest<List<ProvinceDto>>
    {
        public GetProvincesQuery()
        {

        }
    }
    public class GetCitiesByProvinceIdQuery : IRequest<List<CityDto>>
    {
        public int ProvinceId { get; set; }
        public GetCitiesByProvinceIdQuery(int ProvinceId)
        {
            this.ProvinceId = ProvinceId;
        }
    }
    public class GetCityByIdQuery : IRequest<City>
    {
        public int CityId { get; set; }
        public GetCityByIdQuery(int CityId)
        {
            this.CityId = CityId;
        }
    }

    public class GetProvinceByIdQuery : IRequest<Province>
    {
        public int ProvinceId { get; set; }
        public GetProvinceByIdQuery(int ProvinceId)
        {
            this.ProvinceId = ProvinceId;
        }
    }
}
