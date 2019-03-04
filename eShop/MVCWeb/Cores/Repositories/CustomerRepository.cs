using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;

namespace MVCWeb.Cores.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly IDbAppContext _context;
        public CustomerRepository(IDbAppContext context) : base(context)
        {
            _context = context as DbAppContext;
        }
    }
}