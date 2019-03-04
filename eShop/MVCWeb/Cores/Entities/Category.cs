using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Cores.Entities
{
    public partial class Category
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
        public Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}