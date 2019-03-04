using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWeb.AppDataLayer.Entities
{
    [Table("ProductImport")]
    public class ProductImport
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }
        public virtual ICollection<ProductImportDetail> ProductImportDetails { get; set; }
    }
}