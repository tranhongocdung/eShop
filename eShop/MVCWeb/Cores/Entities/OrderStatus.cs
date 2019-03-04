using System.Collections.Generic;

namespace MVCWeb.Cores.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        //Enum
        public const int Pending = 1;
        public const int Completed = 2;
        public const int Cancelled = 3;
    }
}