using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;

namespace MVCWeb.Cores.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository
            )
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public int Create(Order order)
        {
            order.CreatedOn = DateTime.Now;
            order.OrderStatusId = OrderStatus.Pending;
            _orderRepository.Insert(order);
            return order.Id;
        }
        public bool UpdateOrder(Order order)
        {
            var currentOrder = _orderRepository.GetById(order.Id);
            if (currentOrder == null) return false;
            currentOrder.CustomerId = order.CustomerId;
            currentOrder.Note = order.Note;
            currentOrder.DiscountValue = order.DiscountValue;
            currentOrder.DiscountType = order.DiscountType;
            _orderRepository.Update(order);
            return true;
        }

        public void CompleteOrder(int orderId)
        {
            var order = GetWithOrderDetails(orderId);
            if (order == null) return;
            order.OrderStatusId = OrderStatus.Completed;
            var totalCash = (decimal)order.OrderDetails.Sum(o => o.SellingPrice * o.Quantity);
            decimal realCash = totalCash -
                           (order.DiscountType == 0 ? totalCash * order.DiscountValue / 100 : order.DiscountValue);
            order.CompletedRealCash = realCash;
            _orderRepository.Update(order);
        }

        public void CancelOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null) return;
            order.OrderStatusId = OrderStatus.Cancelled;
            _orderRepository.Update(order);
        }
        public void RestoreOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null) return;
            order.OrderStatusId = OrderStatus.Pending;
            _orderRepository.Update(order);
        }

        public Order GetWithCustomerAndOrderDetails(int id)
        {
            return
                _orderRepository.TableNoTracking.Include(o => o.Customer)
                    .Include(o => o.OrderDetails.Select(p => p.ProductVariant.Product))
                    .FirstOrDefault(o => o.Id == id);
        }

        public Order GetWithOrderDetails(int id)
        {
            return _orderRepository.Table.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);
        }

        public Order GetWithAllRelations(int id)
        {
            return
                _orderRepository.TableNoTracking.Include(o => o.Customer)
                    .Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetList(FilterParams fp, ref int totalCount)
        {
            var list = _orderRepository.TableNoTracking.Include(o => o.Customer).Include(o => o.CreatedBy)
                .Include(o => o.OrderDetails.Select(p => p.ProductVariant.Product));
            if (fp.StatusId != 0)
            {
                list = list.Where(o => o.OrderStatusId == fp.StatusId);
            }
            if (!string.IsNullOrEmpty(fp.FromDate))
            {
                var fromDate = DateTime.ParseExact(fp.FromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                list = list.Where(o => o.CreatedOn >= fromDate);
            }
            if (!string.IsNullOrEmpty(fp.ToDate))
            {
                var toDate = DateTime.ParseExact(fp.ToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                list = list.Where(o => o.CreatedOn < toDate);
            }
            if (fp.CustomerIds.Any())
            {
                list = list.Where(o => fp.CustomerIds.Contains(o.CustomerId));
            }
            totalCount = list.Count();
            list = list.OrderBy(fp.SortField + (fp.SortASC ? " ASC" : " DESC"));
            if (fp.PageNumber == 0) return list.ToList();
            var skip = (fp.PageNumber - 1) * fp.PageSize;
            var take = fp.PageSize;
            list = list.Skip(skip).Take(take);
            return list.ToList();
        }


        public void UpdateOrderDetail(List<OrderDetail> orderDetails, int orderId)
        {
            //Delete
            var newOrderDetailIds = orderDetails.Where(o => o.Id != 0).Select(o => o.Id);
            var removedOrderDetails = _orderDetailRepository.Table.Where(o => o.OrderId == orderId && !newOrderDetailIds.Contains(o.Id));
            _orderDetailRepository.Delete(removedOrderDetails);
            //Update and add new
            orderDetails.ForEach(orderDetail =>
            {
                var currentOrderDetail = _orderDetailRepository.GetById(orderDetail.Id);
                if (currentOrderDetail != null)
                {
                    currentOrderDetail.ProductVariantId = orderDetail.ProductVariantId;
                    currentOrderDetail.SellingPrice = orderDetail.SellingPrice;
                    currentOrderDetail.Quantity = orderDetail.Quantity;
                    currentOrderDetail.Note = orderDetail.Note;
                    _orderDetailRepository.Update(currentOrderDetail);
                }
                else
                {
                    orderDetail.OrderId = orderId;
                    _orderDetailRepository.Insert(orderDetail);
                }
            });
        }
    }
}