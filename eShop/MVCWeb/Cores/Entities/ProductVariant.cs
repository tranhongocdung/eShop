using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Cores.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        public int DefaultSellingPrice { get; set; }
        public virtual Colour Colour { get; set; }
        public virtual Size Size { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}