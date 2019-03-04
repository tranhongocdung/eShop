using System;
using System.Collections.Generic;

namespace MVCWeb.Cores.Entities
{
    public partial class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public int DiscountType { get; set; }
        public int DiscountValue { get; set; }
        public int OrderStatusId { get; set; }
        public int? CreatedById { get; set; }
        public decimal CompletedRealCash { get; set; }
        public virtual Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public User CreatedBy { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}