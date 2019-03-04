using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;

namespace MVCWeb.Cores.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly IDbAppContext _context;

        public OrderRepository(IDbAppContext context) : base(context)
        {
            _context = context as DbAppContext;
        }
        
    }
}