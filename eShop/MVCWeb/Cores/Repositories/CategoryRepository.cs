using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;

namespace MVCWeb.Cores.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly IDbAppContext _context;
        public CategoryRepository(IDbAppContext context) : base(context)
        {
            _context = context as DbAppContext;
        }
    }
}