using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Models
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}