using System.Collections.Generic;

namespace Sparta_Online_Shop
{
    public class OrderPageModel
    {
        public Order order { get; set; }
        public User user { get; set; }
        public OrderStatu orderStatus { get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }
        public IEnumerable<Product> orderProducts { get; set; }
    }
}