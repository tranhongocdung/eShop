using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Models
{
    public class CustomerManageViewModel : BasePagingViewModel
    {
        public string Keyword { get; set; }
        public int IsVIP { get; set; }
        public List<Customer> Customers { get; set; }
    }
}