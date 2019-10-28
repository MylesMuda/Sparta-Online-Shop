using System.Collections.Generic;

namespace Sparta_Online_Shop
{
    public class OrderPageModel
    {
        public Order order { get; set; }
        public User user { get; set; }
        public OrderStatu orderStatus { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
        public List<Product> orderProducts { get; set; }
    }
}