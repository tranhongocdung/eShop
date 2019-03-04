using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWeb.Cores.Entities
{
    public partial class Category
    {
        [NotMapped]
        public bool IsChild => (ParentId != null);
    }
}