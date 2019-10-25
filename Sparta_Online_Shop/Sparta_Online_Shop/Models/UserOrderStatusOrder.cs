using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparta_Online_Shop
{
    public class UserOrderStatusOrder
    {
        public User user { get; set; }
        public List<Order> orders { get; set; }
        public List<OrderStatu> orderStatuses { get; set; }

    }
}