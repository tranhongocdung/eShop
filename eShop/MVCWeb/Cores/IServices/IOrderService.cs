using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.IServices
{
    public interface IOrderService : IWebAppService
    {
        int Create(Order order);
        bool UpdateOrder(Order order);
        void CompleteOrder(int orderId);
        void CancelOrder(int orderId);
        void RestoreOrder(int orderId);
        Order GetWithOrderDetails(int id);
        Order GetWithCustomerAndOrderDetails(int id);
        Order GetWithAllRelations(int id);
        List<Order> GetList(FilterParams fp, ref int totalCount);
        void UpdateOrderDetail(List<OrderDetail> orderDetails, int orderId);

    }
}
