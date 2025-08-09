using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Carts
{

    public class Cart
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public long TotalPrice => Items.Sum(i => i.Price * i.Quantity);

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
        public int VariantId { get; set; }
        public string ProductName { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }

        public CartItem(ProductVariant variant, int quantity)
        {
            VariantId = variant.Id;
            ProductName = variant.Product.Name;
            Price = variant.Price;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount > 0) Quantity += amount;
        }
    }
}
