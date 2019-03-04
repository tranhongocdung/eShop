using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Models;

namespace MVCWeb.Controllers
{
    //[CustomAuthorize(Roles = "Admin")]
    public class DataSourceController : BaseController
    {
        private readonly IProductVariantRepository _productRepository;
        public DataSourceController(
            IProductVariantRepository productRepository
        )
        {
            _productRepository = productRepository;
        }
        public ActionResult GetProductName(string query, int id = 0)
        {
            var db = new DbAppContext();
            if (id != 0)
            {
                var item = _productRepository.Table
                    .Include(o => o.Product)
                    .Include(o => o.Size)
                    .Include(o => o.Colour).First(o => o.Id == id);
                return Json(new SuggestProductViewModel
                {
                    ProductId = item.Id,
                    ShortDescription = item.Product.ShortDescription,
                    UnitPrice = item.DefaultSellingPrice,
                    ProductName = item.Product.ProductName,
                    ProductOptions = "Size: " + item.Size.Name + ", Colour: " + item.Colour.Name
                }, JsonRequestBehavior.AllowGet);
            }

            var list = _productRepository.Table.Include(o => o.Product)
                .Include(o => o.Size)
                .Include(o => o.Colour).Where(o => o.Product.ProductName.Contains(query));
            return Json(list.Select(item => new SuggestProductViewModel
            {
                ProductId = item.Id,
                ShortDescription = item.Product.ShortDescription,
                UnitPrice = item.DefaultSellingPrice,
                ProductName = item.Product.ProductName + " (" + item.Size.Name + ", " + item.Colour.Name + ")",
                ProductOptions = "Size: " + item.Size.Name + ", Colour: " + item.Colour.Name
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerSuggestion(string query, int id = 0)
        {
            var db = new DbAppContext();
            if (id != 0)
            {
                var item = db.Customers.First(o => o.Id == id);
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            var list =
                db.Customers.Where(
                    o => o.CustomerName.Contains(query) || o.Email.Contains(query) || o.PhoneNo.Contains(query)).Take(10).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}