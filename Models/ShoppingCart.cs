using System.Collections.Generic;
using System.Linq;

namespace website_ban_hang.Models
{
    public class ShoppingCart
    {
        // Danh sách các mặt hàng trong giỏ
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Hàm thêm sản phẩm vào giỏ
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity; // Nếu có rồi thì tăng số lượng
            }
            else
            {
                Items.Add(item); // Chưa có thì thêm mới
            }
        }

        // Hàm xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }
}