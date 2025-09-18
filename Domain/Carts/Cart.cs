using Domain.Products;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Carts
{
    public enum CartType
    {
        Open = 1,
        Next = 2,
        Closed = 3,
    }
    public class Cart
    {
        public Cart()
        {

        }
        public long Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public long TotalPrice => Items.Sum(i => (i.LastKnownSpecialPrice is null ? i.LastKnownPrice : (long)i.LastKnownSpecialPrice) * i.Quantity);
        public CartType CartType { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public Cart(string userId)
        {
            UserId = userId;
        }

        public void AddItem(ProductVariant variant, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductVariantId == variant.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
                existingItem.UpdatedAt = DateTime.Now;
                if (Id != 0)
                    UpdatedAt = DateTime.Now;
            }
            else
            {
                Items.Add(new CartItem(variant, quantity));
                if (Id != 0)
                    UpdatedAt = DateTime.Now;
            }
        }

        public void RemoveItem(int variantId)
        {
            var item = Items.FirstOrDefault(i => i.Id == variantId);
            if (item != null) Items.Remove(item);
        }

        // متدهای دیگر مثل ClearCart()
    }

    public class CartItem
    {
        public CartItem()
        {

        }
        public long Id { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public long LastKnownPrice { get; set; }
        public long? LastKnownSpecialPrice { get; set; }
        public int Quantity { get; set; }

        public CartItem(ProductVariant variant, int quantity)
        {
            ProductVariantId = variant.Id;
            LastKnownSpecialPrice = variant.SpecialPrice;
            LastKnownPrice = variant.Price;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount > 0) Quantity += amount;
        }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        //
        public Cart Cart { get; set; }
        public long CartId { get; set; }
    }
}
