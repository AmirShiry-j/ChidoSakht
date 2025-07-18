using Application.Store.UserSection.ProductService.Queries;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Product
{
    public class ProductFilterForUserSectionApiDto
    {
        /// <summary>
        /// 1_View 2_Date 3_Sell 4_Price
        /// </summary>
        [Required]
        [Range(1, 4)]
        public TypeOrderByForProduct TypeOrderByForProduct { get; set; }
        public bool Ascending { get; set; } = false;
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public bool? OnlyAvailableGoods { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }

        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
}
