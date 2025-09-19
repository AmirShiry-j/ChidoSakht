using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Orders
{
    public class CreateOrderApiDto
    {
        public long CartId { get; set; }
        public int AddressId { get; set; }
        [Required]
        [Range(1, 2)]
        public SendByApiEnum SendBy { get; set; }
    }
    public class GetOrdersByFilterApiDto
    {
        [Range(1, 7)]
        //[Required]
        public OrderStatusApiEnum? OrderStatus { get; set; }
    }

    public enum SendByApiEnum
    {
        Tipax = 1,
        Post = 2
    }
    public enum OrderStatusApiEnum
    {
        PendingPayment = 1,//→ در انتظار پرداخت
        Paid = 2,//→ پرداخت موفق
        Processing,//→ در حال آماده‌سازی
        Shipped,//→ ارسال شده
        Delivered,//→ تحویل داده شده
        CanceledByUser, //→ لغو شده توسط یوزر
        CanceledByAdmin //→ لغو شده توسط ادمین
    }
}
