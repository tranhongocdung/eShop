using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.IServices
{
    public interface ICustomerService : IWebAppService
    {
        int Create(Customer customer);
        bool UpdateCustomer(Customer customer);
        List<Customer> GetList(FilterParams fp, ref int totalCount);
    }
}
