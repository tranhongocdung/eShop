using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Cores.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}