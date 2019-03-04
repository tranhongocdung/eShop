using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;

namespace MVCWeb.Cores.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IDbAppContext _context;

        public ProductRepository(IDbAppContext context) : base(context)
        {
            _context = context as DbAppContext;
        }
        
    }
}