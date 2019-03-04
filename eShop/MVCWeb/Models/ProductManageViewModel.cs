using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Models
{
    public class ProductManageViewModel : BasePagingViewModel
    {
        public string Keyword { get; set; }
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}