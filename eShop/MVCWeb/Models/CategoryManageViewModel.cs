using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Models
{
    public class CategoryManageViewModel
    {
        public string Keyword { get; set; }
        public List<Category> Categories { get; set; }
        //public List<Category> ParentCategories { get; set; }
        public CategoryEditViewModel CategoryEditViewModel { get; set; }
    }
}