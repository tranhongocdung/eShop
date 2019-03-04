using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;

namespace MVCWeb.Cores.Repositories
{
    public class ProductVariantRepository : GenericRepository<ProductVariant>, IProductVariantRepository
    {
        private readonly IDbAppContext _context;

        public ProductVariantRepository(IDbAppContext context) : base(context)
        {
            _context = context as DbAppContext;
        }
        
    }
}