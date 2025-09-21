using Domain.Carts;
using Domain.Products;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Orders
{
    public class Order
    {
        public long Id { get; set; }
        public long TotalAmout { get; set; }
        public long DiscountAmout { get; set; }
        public SendBy SendBy { get; set; }
        public long SendCost { get; set; }
        public long FinalAmout { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        //
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long CartId { get; set; }
        public Cart Cart { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public Payment Payment { get; set; }
    }

    public enum OrderStatus
    {
        PendingPayment = 1,//→ در انتظار پرداخت
        Paid = 2,//→ پرداخت موفق
        Processing,//→ در حال آماده‌سازی
        Shipped,//→ ارسال شده
        Delivered,//→ تحویل داده شده
        CanceledByUser, //→ لغو شده توسط یوزر
        CanceledByAdmin //→ لغو شده توسط ادمین
    }
    public enum SendBy
    {
        Tipax = 1,
        Post = 2
    }
    public class OrderItem
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        //
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }

    public class Payment
    {
        public long Id { get; set; }
        public long Amout { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public long TransactionId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        //
        public long OrderId { get; set; }
        public Order Order { get; set; }

    }
    public enum PaymentMethod
    {
        BankPort = 1
    }
    public enum PaymentStatus
    {
        Faild = 1,
        Success = 2,
    };
}