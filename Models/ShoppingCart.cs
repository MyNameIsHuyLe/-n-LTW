using System.Collections.Generic;
using System.Linq;

namespace website_ban_hang.Models
{
    public class ShoppingCart
    {
        // Danh sách sản phẩm trong giỏ hàng
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Thêm sản phẩm vào giỏ hàng
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                Items.Remove(item);
            }
        }

        // Xóa toàn bộ giỏ hàng
        public void Clear()
        {
            Items.Clear();
        }

        // Tổng số lượng sản phẩm
        public int GetTotalQuantity()
        {
            return Items.Sum(i => i.Quantity);
        }

        // Tổng tiền giỏ hàng
        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.Price * i.Quantity);
        }
    }
}