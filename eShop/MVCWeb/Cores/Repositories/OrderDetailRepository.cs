using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;

namespace MVCWeb.Cores.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly IDbAppContext _context;

        public OrderDetailRepository(IDbAppContext context) : base(context)
        {
            _context = context as DbAppContext;
        }
        
    }
}