using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Product
{
    public class ProductFilterForUserSectionApiDto
    {
        [Range(1, 4)]
        public FilterFor FilterFor { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public bool? OnlyAvailableGoods { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
    public enum FilterFor
    {
        Bazdid,
        Jadid,
        Forush,
        Arzan,
    }
}
