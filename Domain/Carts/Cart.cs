using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Carts
{
    public enum CartStatus
    {
        Open,
        Next,
        Closed,
    }
    public class Cart
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public long TotalPrice => Items.Sum(i => i.Price * i.Quantity);
        public CartStatus CartStatus { get; set; }
        public Cart(string userId)
        {
            UserId = userId;
        }

        public void AddItem(ProductVariant variant, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.Id == variant.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                Items.Add(new CartItem(variant, quantity));
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
        public long Id { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }

        public CartItem(ProductVariant variant, int quantity)
        {
            ProductVariantId = variant.Id;
            ProductId = variant.ProductId;
            ProductName = variant.Product.Name;
            Price = variant.SpecialPrice is null ? variant.Price : (long)variant.SpecialPrice;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount > 0) Quantity += amount;
        }
    }
}
