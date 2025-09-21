using System.ComponentModel.DataAnnotations;
using WebApi.ModelsAndDtoes.Orders;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Orders
{
    public class GetOrdersByFilterAdminApiDto
    {
        [Range(1, 7)]
        public OrderStatusApiEnum? OrderStatus { get; set; }
        public string? UserId { get; set; }
    }
}
