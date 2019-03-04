using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCWeb.Cores;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IServices;
using MVCWeb.Cores.Security;
using MVCWeb.Libraries;
using MVCWeb.Models;
using Newtonsoft.Json;

namespace MVCWeb.Controllers
{
    [WhitespaceFilter]
    [CustomAuthorize(Roles = "*")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;

        public OrderController(
            IOrderService orderService,
            ICategoryService categoryService,
            ICustomerService customerService
            )
        {
            _orderService = orderService;
            _customerService = customerService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Manage()
        {
            var model = new OrderManageViewModel()
            {
                CurrentPage = 1,
                PageSize = 10,
            };
            var totalCount = 0;
            model.Orders = _orderService.GetList(new FilterParams()
            {
                SortField = "CreatedOn"
            }, ref totalCount);
            model.ItemCount = totalCount;
            return View(model);
        }
        [HttpPost]
        public ActionResult Manage(OrderManageViewModel model, int page)
        {
            model.CurrentPage = page;
            model.PageSize = 10;
            var totalCount = 0;
            var customerIds = !string.IsNullOrWhiteSpace(model.CustomerIds)
                ? model.CustomerIds.Split(',').Select(int.Parse)
                : new List<int>();
            model.Orders = _orderService.GetList(new FilterParams
            {
                PageNumber = page,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                CustomerIds = customerIds.ToList(),
                SortField = "CreatedOn",
                StatusId = model.StatusId
            }, ref totalCount);
            model.ItemCount = totalCount;
            return View("_OrderTable", model);
        }
        public ActionResult Edit(int id = 0)
        {
            var model = new OrderEditViewModel();
            if (id != 0)
            {
                var order = _orderService.GetWithCustomerAndOrderDetails(id);
                if (order != null)
                {
                    model.Order = order;
                    model.Customer = order.Customer;
                }
            }
            model.Categories = _categoryService.GetAllWithTreeViewOrder();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(OrderEditViewModel model)
        {
            var customerId = model.Customer.Id;
            var orderId = model.Order.Id;
            if (customerId == 0)
            {
                customerId = _customerService.Create(model.Customer);
            }
            else
            {
                _customerService.UpdateCustomer(model.Customer);
            }

            model.Order.CustomerId = customerId;
            var orderDetails = new List<OrderDetail>();
            if (!string.IsNullOrEmpty(model.OrderDetailJson))
            {
                orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(model.OrderDetailJson);
            }

            //Update
            if (model.Order.Id != 0)
            {
                _orderService.UpdateOrder(model.Order);
                _orderService.UpdateOrderDetail(orderDetails, model.Order.Id);
                return Content("");
            }

            //Add new
            model.Order.OrderDetails = orderDetails;
            model.Order.CreatedById = User.UserId;
            orderId = _orderService.Create(model.Order);
            return Content(orderId.ToString());
        }

        public ActionResult Complete(int id)
        {
            _orderService.CompleteOrder(id);
            return Content("");
        }
        public ActionResult Cancel(int id)
        {
            _orderService.CancelOrder(id);
            return Content("");
        }
        public ActionResult Restore(int id)
        {
            _orderService.RestoreOrder(id);
            return Content("");
        }
    }
}