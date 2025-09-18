namespace WebApi.ModelsAndDtoes.Orders
{
    public class CreateOrderApiDto
    {
        public long CartId { get; set; }
        public int AddressId { get; set; }
        public SendByApiEnum SendBy { get; set; }
        public OrderStatusApiEnum OrderStatus  { get; set; }
    }
    public enum SendByApiEnum
    {
        Tipax=1,
        Post=2
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
