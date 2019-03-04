using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Cores.Entities
{
    public partial class Customer
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Note { get; set; }
        public bool IsVIP { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}