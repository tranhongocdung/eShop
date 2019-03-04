using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWeb.AppDataLayer.Entities
{
    [Table("ProductImportDetail")]
    public class ProductImportDetail
    {
        [Key]
        public int Id { get; set; }
        public int ProductImportId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("ProductImportId")]
        public virtual ProductImport ProductImport { get; set; }
    }
}