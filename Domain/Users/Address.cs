using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string FullAddress { get; set; }
        public string Pelak { get; set; }
        public string PostalCode { get; set; }
        public string? UnitNumber { get; set; } 
        public WhoRecept WhoRecept { get; set; }
        public string? NameRecipient { get; set; }
        public string? PhoneNumberRecipient { get; set; }
        //Navs
        public User User { get; set; }
        public string UserId { get; set; }
    }
    public enum WhoRecept
    {
        Me = 1,
        Other = 2
    }

    public class Province
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public ICollection<City> cities { get; set; }
    }
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        //
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}
